using Business.IServices;
using Common.Enums;
using Common.Extensions;
using Core.Context;
using Dtos;
using Entities;
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
        private AgentContext context { get; set; }
        public AuthenticationService(IOptions<AppSettings> _appSettings, AgentContext _context)
        {
            appSettings = _appSettings.Value;
            context = _context;
        }

        public AuthenticationDto Authenticate(string fullName, RoleType roleType)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var user = new User() { FullName = fullName };
            context.Users.Add(user);
            context.SaveChangesAsync();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, fullName),
                     new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                      new Claim(ClaimTypes.Role, roleType.GetDisplayName())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var authenticationDto = new AuthenticationDto() { Token = tokenHandler.WriteToken(token), UserId = user.Id };
            return authenticationDto;
        }
    }
}
