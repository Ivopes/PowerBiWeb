﻿using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<IEnumerable<ApplUser>> GetAsync()
        {
            return await _appUserRepository.GetAllAsync();
        }

        public async Task<string> PostAsync(UserRegisterInformation user)
        {
            ApplUser u = new()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                Password= user.Password,
                AppRole = AppRoles.User
            };
            return await _appUserRepository.PostAsync(u);
        }
    }
}
