﻿using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<string> LoginAsync(User user)
        {
            return await _authRepository.LoginAsync(user);
        }
    }
}