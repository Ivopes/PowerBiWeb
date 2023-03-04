using Microsoft.IdentityModel.Tokens;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Repositories;
using PowerBiWeb.Shared.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PowerBiWeb.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectRepository _projectRepository;
        public AuthService(IAuthRepository authRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor, IProjectRepository projectRepository)
        {
            _authRepository = authRepository;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
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
                new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()),
                new Claim(ClaimTypes.Name, entity.Username),
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
        public async Task<ProjectRoles?> GetProjectRole(int projectId)
        {
            var userId = GetUserId();

            var project = await _projectRepository.GetAsync(projectId);

            if (project is null) return null;
            if (!project.AppUserProjects.Any(aup => aup.AppUserId == userId)) return null;

            var join = project.AppUserProjects.Single(aup => aup.AppUserId == userId);

            return join.Role;
        }
        
        #region Private Methods
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
        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        #endregion
    }
}
