using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Contracts.Services
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateToken(ApplicationUser user);
    }
}
