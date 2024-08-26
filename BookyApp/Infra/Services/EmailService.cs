using FluentEmail.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Settings;
using Application.Contracts.Services;
using Application.DTOs;
using System.Net.Mail;
using System.Net;

namespace Infra.Services
{
    public class EmailService: IEmailService
    {
        private readonly EmailSettings _emailSettings;

         public EmailService(IOptions<EmailSettings> options) {
        _emailSettings = options.Value;
        }
      public async Task<ApiResponse<bool>> sendMailAsync() {
            var response = new ApiResponse<bool>();
            var emailResponse =  Email
        .From("mohamedal3alme6@gmail.com")
        .To("moessamn@gmail.com", "Luke")
        .Subject("Hi Luke!")
        .Body("Fluent email looks great!",true);
            var ss = await emailResponse.SendAsync();
            using (System.Net.Mail.MailMessage mm = new MailMessage("mohamedal3alme6@gmail.com", "moessamn@gmail.com"))
            {
                mm.Subject = "|kkkk";
                mm.Body = "msg.Body";
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    NetworkCredential NetworkCred = new NetworkCredential("mohamedal3alme6@gmail.com", "iejgkxerahxdxeqe");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    await smtp.SendMailAsync(mm);
                    
                }
            }
            // Check the result and return a simple response
            if (ss.Successful)
            {
                 response.Status=true;
            }
            else
            {
                
                response.Status = false;
            }
            return response;

        }
    }
}
