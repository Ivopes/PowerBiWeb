using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PowerBiWeb.Server.Models.Entities;
using NuGet.Packaging;

namespace PowerBiWeb.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        public AuthService(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        public async Task<string> LoginAsync(UserLoginInformation user)
        {
            string token = string.Empty;

            var entity = await _authRepository.LoginAsync(user);

            if (entity is null) return token;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSecretKey")!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
            };

            claims.AddRange(CreateRoleClaims(entity.AppRole));

            var tokenOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
        private List<Claim> CreateRoleClaims(AppRoles role)
        {
            var roleClaims = new List<Claim>();

            switch (role)
            {
                case AppRoles.Admin:
                    roleClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    roleClaims.Add(new Claim(ClaimTypes.Role, "User"));
                    break;
                case AppRoles.User:
                    roleClaims.Add(new Claim(ClaimTypes.Role, "User"));
                    break;
                default:
                    break;
            }

            return roleClaims;
        }
    }
}
