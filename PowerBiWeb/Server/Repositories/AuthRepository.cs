using Microsoft.IdentityModel.Tokens;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PowerBiWeb.Server.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public string Login(User user)
        {
            const string key = "123456123123456123";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(key,
                key,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
