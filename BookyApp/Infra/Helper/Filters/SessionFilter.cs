using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helper.Filters
{
    public class SessionFilter : IAsyncActionFilter
    {
        private readonly Session _session;

        public SessionFilter(Session session)
        {
            _session = session;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsIdentity = (ClaimsIdentity)context.HttpContext.User.Identity;

            _session.UserId = claimsIdentity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            _session.Email = claimsIdentity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            _session.FullName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (!string.IsNullOrWhiteSpace(_session.UserId))
            {
                _session.Role = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            }

            await next();
        }
    }
    public class Session
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        //public List<SessionRole>? Role { get; set; }
    }
}
