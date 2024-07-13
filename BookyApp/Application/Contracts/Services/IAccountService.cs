using Application.DTOs.UserDto;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.Services
{
    public interface IAccountService
    {
       Task<BaseResponse> AddUser(AddUserRequest request);
        Task<AuthResponse> Login(UserLoginRequest request);
        Task<ApplicationUser> GetUserByEmail(string email);
    }
}
