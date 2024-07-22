using Application.Contracts.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Services;
using Application.DTOs;

namespace Infra.Services
{
    class UserInterestsService: IUserInterestsService
    {
        private readonly IBaseRepository<UserInterests> _userInterestsRepository;
        private readonly IBaseRepository<Genres> _interestsRepository;

        public UserInterestsService(IBaseRepository<UserInterests> userInterestsRepository , IBaseRepository<Genres> interestsRepository) {
          _userInterestsRepository = userInterestsRepository;
            _interestsRepository = interestsRepository;
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
        public ApiResponse<Genres> addInterest(string name)
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

            response.Status = true;
            return response;
        }

    }
}
