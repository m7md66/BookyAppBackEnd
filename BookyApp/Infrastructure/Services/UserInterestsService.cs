using Application.Contracts.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Services;
using Application.DTOs;

namespace Infrastructure.Services
{
    class UserInterestsService: IUserInterestsService
    {
        private readonly IBaseRepository<UserInterests> _userInterestsRepository;
        private readonly IBaseRepository<Interest> _interestsRepository;

        public UserInterestsService(IBaseRepository<UserInterests> userInterestsRepository , IBaseRepository<Interest> interestsRepository) {
          _userInterestsRepository = userInterestsRepository;
            _interestsRepository = interestsRepository;
        }

        public ApiResponse<Book> GetUserInterests(string UserId)
        {
            var response = new ApiResponse<Book>();

          
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
        public ApiResponse<Book> addInterests(string UserId)
        {
            var response = new ApiResponse<Book>();


            try
            {
              var result =_interestsRepository.Add
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
