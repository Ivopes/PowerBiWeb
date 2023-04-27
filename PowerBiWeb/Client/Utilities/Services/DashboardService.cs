using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Projects;
using System.Net.Http;
using System.Net.Http.Json;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class DashboardService : IDashboardService
    {
        private HttpClient _httpClient;
        private ILogger<DashboardService> _logger;
        public DashboardService(HttpClient httpClient, ILogger<DashboardService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<HttpResponse<DashboardDTO?>> GetDashboardAsync(int projectId, Guid dashboardId)
        {
            return await GetDashboardAsync(projectId, dashboardId, CancellationToken.None);
        }
        public async Task<HttpResponse<DashboardDTO?>> GetDashboardAsync(int projectId, Guid dashboardId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/dashboards/{projectId}/{dashboardId}", ct);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var dashboard = await response.Content.ReadFromJsonAsync<DashboardDTO>();
                    return new()
                    {
                        IsSuccess = true,
                        Value = dashboard,
                        ErrorMessage = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
            }

            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Value = null
            };
        }

        public async Task<HttpResponse> UpdateDashboardSettingsAsync(DashboardDTO dashboard)
        {
            return await UpdateDashboardSettingsAsync(dashboard, CancellationToken.None);
        }
        public async Task<HttpResponse> UpdateDashboardSettingsAsync(DashboardDTO dashboard, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/dashboards", dashboard, ct);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return new()
                    {
                        IsSuccess = true,
                        ErrorMessage = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "dashboard could not been changed");
                }
            }

            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
    }
}
