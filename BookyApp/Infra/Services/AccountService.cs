using Application.DTOs;
using Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.UserDto;
using static System.Collections.Specialized.BitVector32;
using Application.Contracts.Repository;
using Application.Contracts.Services;
using Application.DTOs.Settings;
using Infra.Helper.Filters;
using Application;
using Application.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Infra.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly Session _session;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IEmailService _emailService;
        private readonly IBaseRepository<EmailOtp> _emailOtpRepo;
        private readonly FeatureFlags _featureFlags;

        private const int OtpLifetimeMinutes = 10;
        private const int OtpResendCooldownSeconds = 60;
        private const int OtpMaxAttempts = 5;
        //private readonly IMapper _mapper;
        //private readonly IUserRepository _userRepository;
        //private readonly IAmazonFileService _amazonService;
        //private readonly RoleManager<ApplicationRole> _roleManager;
        //private readonly IAmazonFileService _amazonFileService;
        //private readonly IFileHandlerService _fileHandlerService;
        public AccountService(UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            Session session,
            IStringLocalizer<SharedResource> localizer,
            IEmailService emailService,
            IBaseRepository<EmailOtp> emailOtpRepo,
            IOptions<FeatureFlags> featureFlags
            //IMapper mapper
            //IUserRepository userRepository,
            //IAmazonFileService amazonService,
            //RoleManager<ApplicationRole> roleManager,
            //IAmazonFileService amazonFileService,
            //IFileHandlerService fileHandlerService
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _session = session;
            _localizer = localizer;
            _emailService = emailService;
            _emailOtpRepo = emailOtpRepo;
            _featureFlags = featureFlags.Value;
            //_mapper = mapper;
            //_userRepository = userRepository;
            //_amazonService = amazonService;
            //_roleManager = roleManager;
            //_amazonFileService = amazonFileService;
            //_fileHandlerService = fileHandlerService;
        }

        public async Task<UserResponse> GetMyProfile()
        {
            var user = await _userManager.FindByIdAsync(_session.UserId);
            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
                ImageName = user.ImageName,
                ImageExtention = user.ImageExtention
            };
        }

        //public async Task<List<RolesDto>> GetAllRoles()
        //{
        //    return await _roleManager.Roles
        //               .Select(x => new RolesDto
        //               {
        //                   Name = x.Name
        //               })
        //               .ToListAsync();
        //}

        public async Task<BaseResponse> AddUser(AddUserRequest request)
        {
           
            if (await GetUserByEmail(request.Email) is not null)
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = _localizer[MessageKeys.EmailAlreadyRegistered]
                };

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                ImageExtention="request.ImageExtention",
                ImageName="request",
                ImageUrl="request.ImageUrl",
                
                //CreatedBy = _session.UserId
            };

        

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ValidationErrors = result.Errors.Select(err => new ValidationError { Name = err.Code, Description = LocalizeIdentityError(err) }).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            var requiresEmailConfirmation = _featureFlags.RequireEmailConfirmation;
            if (requiresEmailConfirmation)
            {
                // Don't fail registration if SMTP is momentarily unavailable -
                // the client can trigger ResendOtp.
                try { await GenerateAndSendOtpAsync(user); }
                catch { /* logged upstream; user can resend */ }
            }

            return new AddUserResponse((int)HttpStatusCode.OK, true, _localizer[MessageKeys.UserAddedSuccessfully])
            {
                RequiresEmailConfirmation = requiresEmailConfirmation
            };
        }

        public async Task<AuthResponse> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user = await GetUserByEmail(request.Email);
            if (user is null)
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = _localizer[MessageKeys.OtpInvalid]
                };

            if (user.EmailConfirmed)
                return await BuildAuthResponse(user);

            var otps = await _emailOtpRepo.GetManyAsync(o => o.UserId == user.Id && o.ConsumedAt == null);
            var otp = otps.OrderByDescending(o => o.CreatedDate).FirstOrDefault();

            if (otp is null)
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = _localizer[MessageKeys.OtpInvalid]
                };

            if (otp.ExpiresAt < DateTime.UtcNow)
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = _localizer[MessageKeys.OtpExpired]
                };

            otp.Attempts++;

            if (otp.Attempts > OtpMaxAttempts || otp.CodeHash != HashCode(request.Code?.Trim()))
            {
                await _emailOtpRepo.SaveChangesAsync();
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ResponseMessage = _localizer[MessageKeys.OtpInvalid]
                };
            }

            otp.ConsumedAt = DateTime.UtcNow;
            await _emailOtpRepo.SaveChangesAsync();

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var response = await BuildAuthResponse(user);
            response.ResponseMessage = _localizer[MessageKeys.EmailConfirmedSuccess];
            return response;
        }

        public async Task<BaseResponse> ResendOtp(ResendOtpRequest request)
        {
            var user = await GetUserByEmail(request.Email);

            // Do not reveal whether the email exists.
            if (user is null || user.EmailConfirmed)
                return new BaseResponse((int)HttpStatusCode.OK, true, _localizer[MessageKeys.OtpEmailSubject]);

            var otps = await _emailOtpRepo.GetManyAsync(o => o.UserId == user.Id);
            var last = otps.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            if (last?.CreatedDate is DateTime created &&
                created.AddSeconds(OtpResendCooldownSeconds) > DateTime.UtcNow)
            {
                return new BaseResponse((int)HttpStatusCode.BadRequest, false, _localizer[MessageKeys.OtpResendTooSoon]);
            }

            await GenerateAndSendOtpAsync(user);
            return new BaseResponse((int)HttpStatusCode.OK, true, _localizer[MessageKeys.OtpEmailSubject]);
        }

        private async Task<AuthResponse> BuildAuthResponse(ApplicationUser user)
        {
            var jwtSecurityToken = await _tokenService.CreateToken(user);
            return new AuthResponse
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo.ToString("yyyy-MM-dd")
            };
        }

        private async Task GenerateAndSendOtpAsync(ApplicationUser user)
        {
            // Invalidate any codes still outstanding for this user.
            var outstanding = await _emailOtpRepo.GetManyAsync(o => o.UserId == user.Id && o.ConsumedAt == null);
            foreach (var old in outstanding)
                old.ConsumedAt = DateTime.UtcNow;

            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

            _emailOtpRepo.Add(new EmailOtp
            {
                UserId = user.Id,
                CodeHash = HashCode(code),
                ExpiresAt = DateTime.UtcNow.AddMinutes(OtpLifetimeMinutes)
            });
            await _emailOtpRepo.SaveChangesAsync();

            var subject = _localizer[MessageKeys.OtpEmailSubject];
            var body = string.Format(_localizer[MessageKeys.OtpEmailBody], code);
            await _emailService.SendAsync(user.Email, subject, body);
        }

        private static string HashCode(string code)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(code ?? string.Empty));
            return Convert.ToHexString(bytes);
        }




        //momo@gmail.co
        //    aaa2222dddQQ
        public async Task<AuthResponse> Login(UserLoginRequest request)
        {
            var authReponse = new AuthResponse();

            var user = await GetUserByEmail(request.Email);

            //if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            if (user is null )
            {
                authReponse.IsSuccess = false;
                authReponse.StatusCode = (int)HttpStatusCode.BadRequest;
                authReponse.ResponseMessage = _localizer[MessageKeys.InvalidCredentials];
                return authReponse;
            }

            var jwtSecurityToken = await _tokenService.CreateToken(user);

            authReponse.IsSuccess = true;
            authReponse.StatusCode = (int)HttpStatusCode.OK;
            authReponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authReponse.Email = user.Email;
            authReponse.ExpiresOn = jwtSecurityToken.ValidTo.ToString("yyyy-MM-dd");

            return authReponse;
        }


        ////public async Task<GetUserProfileResponse> GetUserDetails(GetUserDetailsProfileRequest getUserDetailsProfileRequest)
        ////{
        ////    return await _userRepository.GetUserByIdAsync(getUserDetailsProfileRequest);

        ////    var x = new GetUserProfileResponse();
        ////    return x;

        ////}

        //public async Task<List<ApplicationUserDto>> GetAdminAndEditors()
        //{
        //    var users = await _userManager.Users
        //                        .Include(x => x.UserRoles)
        //                        .ThenInclude(x => x.Role)
        //                        .ToListAsync();

        //    return _mapper.Map<List<ApplicationUserDto>>(users);
        //}

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // IdentityError.Code is culture-independent and stable; map it to a localized message,
        // falling back to the framework's English description when no resource entry exists.
        private string LocalizeIdentityError(IdentityError err)
        {
            var localized = _localizer[$"Identity_{err.Code}"];
            return localized.ResourceNotFound ? err.Description : localized.Value;
        }

        //public async Task<GetUpdateUserProfileResponse> UpdateUserDetails(UpdateUserRequest updateUserRequest)
        //{

        //    var userDetails = await _userManager.FindByIdAsync(updateUserRequest.Id);
        //    if (userDetails == null)
        //        return new GetUpdateUserProfileResponse
        //        {
        //            StatusCode = 400,
        //            IsSuccess = false,
        //            ResponseMessage = "User Not Exist"
        //        };

        //    if (updateUserRequest.ProfileImage is not null)
        //    {
        //        var deleteOldImageResponse = await _amazonFileService.DeleteFileFromS3(userDetails.ImageName);

        //        var uploadImageResponse = await _fileHandlerService.UploadFile(updateUserRequest.ProfileImage);
        //        if ((bool)!uploadImageResponse.IsSuccess)
        //            return new GetUpdateUserProfileResponse
        //            {
        //                StatusCode = uploadImageResponse.StatusCode,
        //                IsSuccess = false,
        //                ResponseMessage = uploadImageResponse.ResponseMessage
        //            };
        //    }
        //    if (updateUserRequest.NewPassword is not null)
        //    {
        //        IdentityResult? updatePasswordResponse = await _userManager.ChangePasswordAsync(userDetails, updateUserRequest.CurrentPassword, updateUserRequest.NewPassword);
        //        if (!updatePasswordResponse.Succeeded)
        //            return new GetUpdateUserProfileResponse
        //            {
        //                StatusCode = 400,
        //                IsSuccess = false,
        //                ValidationErrors = mappErrors(updatePasswordResponse.Errors.ToList())
        //            };
        //    }

        //    //mapped new data
        //    userDetails.FirstName = updateUserRequest.FirstName;
        //    userDetails.LastName = updateUserRequest.LastName;
        //    userDetails.Email = updateUserRequest.Email;

        //    _userManager.UpdateAsync(userDetails);

        //    var userUpdated = await _userManager.FindByIdAsync(updateUserRequest.Id);
        //    return userUpdated.Adapt<GetUpdateUserProfileResponse>();

        //}

        //private List<ValidationError> mappErrors(List<IdentityError> errors)
        //{
        //    var validations = new List<ValidationError>();

        //    foreach (var error in errors)
        //    {

        //        validations.Add(new ValidationError
        //        {
        //            Name = error.Code,
        //            Description = error.Description,
        //        });
        //    }

        //    return validations;
        //}

        //public async Task<BaseResponse> ResetPassword(ResetPasswordRequest request)
        //{
        //    var user = await _userManager.FindByNameAsync(request.Email);

        //    if (user is null)
        //    {
        //        return new BaseResponse((int)HttpStatusCode.BadRequest, false, "Your not registered yet");
        //    }

        //var resetPasswordResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        //    if (!resetPasswordResult.Succeeded)
        //    {
        //        return new BaseResponse
        //        {
        //            IsSuccess = false,
        //            StatusCode = (int)HttpStatusCode.BadRequest,
        //            ValidationErrors = resetPasswordResult.Errors
        //                            .Select(err => new ValidationError { Name = err.Code, Description = err.Description }).ToList()
        //        };
        //    }

        //    return new BaseResponse((int)HttpStatusCode.OK, true, $"Password changed successfuly");
        //}
    }

}
