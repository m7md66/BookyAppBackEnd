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

namespace Infra.Services
{
    class UserInterestsService: IUserInterestsService
    {
        private readonly IBaseRepository<UserInterest> _userInterestsRepository;
        private readonly IBaseRepository<Genres> _interestsRepository;
        private readonly IBaseRepository<ApplicationUser> _applicationUserRepository;
        private readonly Session _session;
        public UserInterestsService(IBaseRepository<UserInterest> userInterestsRepository , IBaseRepository<Genres> interestsRepository,Session session) {
          _userInterestsRepository = userInterestsRepository;
            _interestsRepository = interestsRepository;
            _session = session;
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
        public async Task<ApiResponse<Genres>> GetAllInterests()
        {
            var response = new ApiResponse<Genres>();


            try
            {
                response.DataResult = await _interestsRepository.GetAllAsync();
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


        public async Task<ApiResponse<bool>> MakeInterests(Guid GenreId)
        {

            var response = new ApiResponse<bool>();
            try
            {
               var theUser=await _applicationUserRepository.FindById(_session.UserId);
                if (theUser is ApplicationUser user) { 
                
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
