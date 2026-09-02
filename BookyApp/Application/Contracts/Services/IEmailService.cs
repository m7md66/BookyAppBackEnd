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

        /// <summary>Sends a single HTML email. Returns true when the SMTP send succeeded.</summary>
        Task<bool> SendAsync(string toEmail, string subject, string htmlBody);
    }
}
