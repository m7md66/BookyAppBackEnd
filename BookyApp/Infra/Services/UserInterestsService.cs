using Application.Contracts.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Services;
using Application.DTOs;
using Infra.Helper.Filters;
using Mapster;
using Application.DTOs.interests;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Infra.Persistence;
using Application;
using Application.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Infra.Services
{
    class UserInterestsService: IUserInterestsService
    {
      
     private readonly AppDbContext _appDb;
        private readonly IBaseRepository<UserInterest> _userInterestsRepository;
        private readonly IBaseRepository<Genres> _interestsRepository;
        private readonly IBaseRepository<ApplicationUser> _applicationUserRepository;
        private readonly Session _session;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public UserInterestsService(AppDbContext appDb,
            IBaseRepository<UserInterest> userInterestsRepository
            , IBaseRepository<Genres> interestsRepository
            ,Session session
            , IBaseRepository<ApplicationUser> applicationUserRepository
            , IStringLocalizer<SharedResource> localizer
          ) {
          _userInterestsRepository = userInterestsRepository;
            _interestsRepository = interestsRepository;
            _session = session;

            _applicationUserRepository = applicationUserRepository;
            _appDb = appDb;
            _localizer = localizer;
        }

        public ApiResponse<List<InterestsResponse>> GetUserInterests(string UserId)
        {
            var response = new ApiResponse<List<InterestsResponse>>();
            try
            {
                var isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";
                response.Data = _userInterestsRepository.GetMany(a => a.UserId == UserId)
                    .Select(x => x.Interest)
                    .ToList()
                    .Select(g => new InterestsResponse
                    {
                        Id = g.Id,
                        Name = isArabic && !string.IsNullOrWhiteSpace(g.NameAr) ? g.NameAr : g.Name
                    })
                    .ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }

            response.Status = true;
            return response;
        }


        public async Task<ApiResponse<List<InterestsResponse>>> GetAllInterests()
        {
            var response = new ApiResponse<List<InterestsResponse>>();
            try
            {
              var generes = await _interestsRepository.GetAllAsync();
               var isArabic = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";
               response.Data = generes
                   .Select(g => new InterestsResponse
                   {
                       Id = g.Id,
                       Name = isArabic && !string.IsNullOrWhiteSpace(g.NameAr) ? g.NameAr : g.Name
                   })
                   .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;
            }

            response.Status = true;
            return response;
        }


        public async Task<ApiResponse<InterestsResponse>> addInterest(string name)
        {
            var response = new ApiResponse<InterestsResponse>();
            var trimmedName = name?.Trim();

            if (string.IsNullOrWhiteSpace(trimmedName))
            {
                response.Errors.Add(_localizer[MessageKeys.InterestNameEmpty]);
                response.Status = false;
                return response;
            }

            try
            {
                var existing = _interestsRepository.GetMany(g => g.Name.ToLower() == trimmedName.ToLower()).FirstOrDefault();
                if (existing != null)
                {
                    response.Data = new InterestsResponse { Id = existing.Id, Name = existing.Name };
                    response.Status = true;
                    return response;
                }

                var genre = new Genres { Name = trimmedName };
                _interestsRepository.Add(genre);
                await _interestsRepository.SaveChangesAsync();
                response.Data = new InterestsResponse { Id = genre.Id, Name = genre.Name };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;
            }
            response.Status = true;
            return response;
        }

        public async Task<ApiResponse<bool>> MakeInterest(Guid GenreId) {

            var response = new ApiResponse<bool>();
            try
            {
                _userInterestsRepository.Add(new UserInterest { InterestId=GenreId,UserId=_session.UserId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;
            }
            await _userInterestsRepository.SaveChangesAsync();
            response.Status = true;
            return response;
        }

            

        public async Task<ApiResponse<bool>> MakeInterests(List<Guid> userInterestsIds)
        {
            var response = new ApiResponse<bool>();
            try
            {
                if (!userInterestsIds.Any())
                {
                    response.Errors.Add(_localizer[MessageKeys.InterestIdsEmpty]);
                    response.Status = false;
                    return response;
                }
               
                //var theUser=await _applicationUserRepository.FindById(_session.UserId);
                var theUser = _applicationUserRepository.GetDb().Where(a => a.Id == _session.UserId).Include(a => a.UserInterests).FirstOrDefault();
                if (theUser is ApplicationUser user) {
                  if (theUser.UserInterests!= null)   theUser.UserInterests.Clear();
                    //await _applicationUserRepository.SaveChangesAsync();
                }
                foreach (var userInterest in userInterestsIds)
                {

                    theUser.UserInterests.Add(new UserInterest { UserId=_session.UserId,InterestId= userInterest });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;
            }
            await _userInterestsRepository.SaveChangesAsync();
            response.Status = true;
            return response;
        }

    }
}
