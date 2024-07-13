using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Services;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
        }

        public async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            //var userClaims = await _userManager.GetClaimsAsync(user);


            var claims = new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Name, user.FullName),
              new Claim("Profile-Picture", user.ImageUrl?? "")
          };

            //foreach (var role in userRoles)
            //    claims.Add(new Claim("Role", role));

            claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var mkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateRandomJwtKey()));
            var symmetricSecurityKey = key;
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_config["JWT:DurationInDays"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        string GenerateRandomJwtKey()
        {
            byte[] key = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
    }
}
