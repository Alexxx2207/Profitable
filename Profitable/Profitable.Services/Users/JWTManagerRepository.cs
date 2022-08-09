using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Profitable.Common.Models;
using Profitable.Services.Users.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Profitable.Services.Users
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration iconfiguration;

        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }

        public JWTToken Authenticate(AuthUserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(iconfiguration["JWT_Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>()
                {
                    { ClaimTypes.NameIdentifier, user.Guid },
                    { ClaimTypes.Name, user.UserName },
                    { ClaimTypes.Email, user.Email },
                },
                //Expires = DateTime.UtcNow.AddDays(GlobalControllerConstants.JWTExpirationInDays),
                Expires = DateTime.UtcNow.AddSeconds(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JWTToken { Token = tokenHandler.WriteToken(token) };
        }
    }
}
