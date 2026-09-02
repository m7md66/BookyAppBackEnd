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
        Task<AuthResponse> ConfirmEmail(ConfirmEmailRequest request);
        Task<BaseResponse> ResendOtp(ResendOtpRequest request);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<UserResponse> GetMyProfile();
    }
}
