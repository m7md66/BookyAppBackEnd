using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Contracts.Services
{
    public interface IEmailService
    {
        Task<ApiResponse<bool>> sendMailAsync();
    }
}
