using Application.DTOs;
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
        ApiResponse<Genres> addInterest(string name);
        Task<ApiResponse<Genres>> GetAllInterests();
        ApiResponse<Genres> GetUserInterests(string UserId);
    }
}
