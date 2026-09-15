using Application.DTOs;
using Application.DTOs.interests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Services
{
    public interface IUserInterestsService
    {
        Task<ApiResponse<InterestsResponse>> addInterest(string name);
        Task<ApiResponse<List<InterestsResponse>>> GetAllInterests();
        ApiResponse<List<InterestsResponse>> GetUserInterests(string UserId);
        Task<ApiResponse<bool>> MakeInterest(Guid GenreId);
        Task<ApiResponse<bool>> MakeInterests(List<Guid> userInterestsIds);
    }
}
