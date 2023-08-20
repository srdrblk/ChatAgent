using Business.IServices;
using Common.Enums;
using Common.Extensions;
using Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings appSettings;

        public AuthenticationService(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings.Value;
        }

        public AuthenticationDto Authenticate(string fullName, RoleType roleType)
        {
            var userId = Guid.NewGuid();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, fullName),
                     new Claim(ClaimTypes.PrimarySid, userId.ToString()),
                      new Claim(ClaimTypes.Role, roleType.GetDisplayName())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var authenticationDto = new AuthenticationDto() { Token = tokenHandler.WriteToken(token), UserId = userId };
            return authenticationDto;
        }
    }
}
