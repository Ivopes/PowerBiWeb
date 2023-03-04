using PowerBiWeb.Client.Pages.Projects;
using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Project;
using PowerBiWeb.Shared.User;
using System.Net.Http.Json;

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
    }
}
