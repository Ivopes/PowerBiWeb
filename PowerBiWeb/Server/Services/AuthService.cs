﻿using Microsoft.IdentityModel.Tokens;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppUserRepository _appUserRepository;
        public AuthService(IAuthRepository authRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor, IProjectRepository projectRepository, IAppUserRepository appUserRepository)
        {
            _authRepository = authRepository;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _appUserRepository = appUserRepository;
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

            return await GetProjectRole(projectId, userId);
        }
        public async Task<ProjectRoles?> GetProjectRole(int projectId, int userId)
        {
            var project = await _projectRepository.GetAsync(projectId);

            if (project is null) return null;
            if (!project.AppUserProjects.Any(aup => aup.AppUserId == userId)) return null;

            var join = project.AppUserProjects.Single(aup => aup.AppUserId == userId);

            return join.Role;
        }
        public async Task<ProjectRoles?> GetProjectRole(int projectId, string userEmail)
        {
            var project = await _projectRepository.GetAsync(projectId);
            var user = await _appUserRepository.GetByEmailAsync(userEmail);

            if (user is null) return null;

            return await GetProjectRole(projectId, user.Id);
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
