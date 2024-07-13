using Application.DTOs;
using Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.UserDto;
using static System.Collections.Specialized.BitVector32;
using Application.Contracts.Services;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        //private readonly IMapper _mapper;
        //private readonly Session _session;
        //private readonly IUserRepository _userRepository;
        //private readonly IAmazonFileService _amazonService;
        //private readonly RoleManager<ApplicationRole> _roleManager;
        //private readonly IAmazonFileService _amazonFileService;
        //private readonly IFileHandlerService _fileHandlerService;
        public AccountService(UserManager<ApplicationUser> userManager,
            ITokenService tokenService
            //IMapper mapper
            //Session session,
            //IUserRepository userRepository,
            //IAmazonFileService amazonService,
            //RoleManager<ApplicationRole> roleManager,
            //IAmazonFileService amazonFileService,
            //IFileHandlerService fileHandlerService
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            //_mapper = mapper;
            //_session = session;
            //_userRepository = userRepository;
            //_amazonService = amazonService;
            //_roleManager = roleManager;
            //_amazonFileService = amazonFileService;
            //_fileHandlerService = fileHandlerService;
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
            var maxSize = 30485760;
            if (await GetUserByEmail(request.Email) is not null)
                return new AuthResponse { ResponseMessage = "Email is already registered!" };

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                ImageExtention=request.ImageExtention,
                ImageName=request.ImageName,
                ImageUrl=request.ImageUrl,
                
                //CreatedBy = _session.UserId
            };

        

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ValidationErrors = result.Errors.Select(err => new ValidationError { Name = err.Code, Description = err.Description }).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, request.Role);
           

            return new BaseResponse((int)HttpStatusCode.OK, true, $"User Added successfuly");
        }




        //momo@gmail.co
        //    aaa2222dddQQ
        public async Task<AuthResponse> Login(UserLoginRequest request)
        {
            var authReponse = new AuthResponse();

            var user = await GetUserByEmail(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                authReponse.IsSuccess = false;
                authReponse.StatusCode = (int)HttpStatusCode.BadRequest;
                authReponse.ResponseMessage = "Email or Password is incorrect!";
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
