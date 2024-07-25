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

namespace Infra.Services
{
    class UserInterestsService: IUserInterestsService
    {
        private readonly IBaseRepository<UserInterest> _userInterestsRepository;
        private readonly IBaseRepository<Genres> _interestsRepository;
        private readonly IBaseRepository<ApplicationUser> _applicationUserRepository;
        private readonly Session _session;
        public UserInterestsService(IBaseRepository<UserInterest> userInterestsRepository
            , IBaseRepository<Genres> interestsRepository
            ,Session session
            , IBaseRepository<ApplicationUser> applicationUserRepository) {
          _userInterestsRepository = userInterestsRepository;
            _interestsRepository = interestsRepository;
            _session = session;
            _applicationUserRepository = applicationUserRepository;
        }

        public ApiResponse<Genres> GetUserInterests(string UserId)
        {
            var response = new ApiResponse<Genres>();
            try
            {
                 response.DataResult = _userInterestsRepository.GetMany(a => a.UserId == UserId).Select(x => new  { x.Interest.Name }).ToList();
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
               response.Data= generes.Adapt<List<InterestsResponse>>();
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


        public async Task<ApiResponse<Genres>> addInterest(string name)
        {
            var response = new ApiResponse<Genres>();


            try
            {
              _interestsRepository.Add(new Genres { Name = name });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;
            }
            await _interestsRepository.SaveChangesAsync();
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
                if (!userInterestsIds.Any()) throw new Exception("User interests IDs cannot be empty.");
               
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
