using FluentEmail.Core;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Settings;
using Application.Contracts.Services;

namespace Infra.Services
{
    public class EmailService: IEmailService
    {
        private readonly EmailSettings _emailSettings;

         public EmailService(IOptionsSnapshot<EmailSettings> options) {
        _emailSettings = options.Value;
        }
      public async Task sendMailAsync() {
            var email = await Email
        .From(_emailSettings.DefaultFromEmail)
        .To("mohamedesamnoman@gmail.com", "Luke")
        .Subject("Hi Luke!")
        .Body("Fluent email looks great!")
        .SendAsync();
        }
    }
}
