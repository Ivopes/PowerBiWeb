﻿using Blazored.Toast.Services;
using PowerBiWeb.Client.Pages.Projects;
using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private ILogger<ReportService> _logger;
        public UserService(HttpClient httpClient, ILogger<ReportService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponse> ChangePassword(string oldPassword, string newPassword)
        {
            return await ChangePassword(oldPassword, newPassword, CancellationToken.None);
        }

        public async Task<HttpResponse> ChangePassword(string oldPassword, string newPassword, CancellationToken ct)
        {
            var request = new ChangePasswordRequest
            {
                NewPassword = newPassword,
                OldPassword = oldPassword
            };
            var response = await _httpClient.PostAsJsonAsync($"api/appusers/password", request, ct);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            };
            return new()
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
        public async Task<HttpResponse<UserDetail>> ChangeUsername(string username)
        {
            return await ChangeUsername(username, CancellationToken.None);
        }
        public async Task<HttpResponse<UserDetail>> ChangeUsername(string username, CancellationToken ct)
        {
            var response = await _httpClient.PostAsync($"api/appusers/username/{username}", null, ct);
            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadFromJsonAsync<UserDetail>();
                return new()
                {
                    Value = value,
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            };
            return new()
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }

        public async Task<HttpResponse<UserDetail>> GetById(int userId)
        {
            var response = await _httpClient.GetAsync($"api/appusers/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserDetail>();

                return new()
                {
                    Value = result,
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync()
            };
        }
    }
}
