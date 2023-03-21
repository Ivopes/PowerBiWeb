using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json.Linq;
using PowerBiWeb.Client.Utilities.Auth;
using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;
        private ILogger<DashboardService> _logger;
        private AuthenticationStateProvider _authProvider;
        public AuthService(HttpClient httpClient, ILogger<DashboardService> logger, AuthenticationStateProvider authProvider)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authProvider = authProvider;
        }
        public async Task<HttpResponse<string>> LoginAsync(string username, string password)
        {
            var request = new UserLoginInformation
            {
                Username = username,
                Password = password
            };
            return await LoginAsync(request);
        }
        public async Task<HttpResponse<string>> LoginAsync(UserLoginInformation loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                return new()
                {
                    Value = token,
                    IsSuccess = true
                };
            }
            return new()
            {
                Value = null,
                ErrorMessage = await response.Content.ReadAsStringAsync()
            };
        }
        public async Task<HttpResponse> RegisterAsync(UserRegisterInformation registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appusers", registerRequest);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync()
            };
        }
    }
}
