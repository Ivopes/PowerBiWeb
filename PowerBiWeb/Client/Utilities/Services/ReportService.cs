using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Project;
using System.Net.Http.Json;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class ReportService : IReportService
    {
        private HttpClient _httpClient;
        private ILogger<ReportService> _logger;
        public ReportService(HttpClient httpClient, ILogger<ReportService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<HttpResponse<EmbedContentDTO[]?>> GetReportsAsync(int projectId)
        {
            return await GetReportsAsync(projectId, CancellationToken.None);
        }
        public async Task<HttpResponse<EmbedContentDTO[]?>> GetReportsAsync(int projectId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}", ct);
            if (response.IsSuccessStatusCode)
            {
                var reports = await response.Content.ReadFromJsonAsync<EmbedContentDTO[]>();
                return new()
                {
                    IsSuccess = true,
                    Value = reports,
                    ErrorMessage = string.Empty
                };
            };
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Value = null
            };
        }
        public async Task<HttpResponse<EmbedContentDTO?>> GetReportAsync(int projectId, Guid reportId)
        {
            return await GetReportAsync(projectId, reportId, CancellationToken.None);
        }
        public async Task<HttpResponse<EmbedContentDTO?>> GetReportAsync(int projectId, Guid reportId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}/{reportId}", ct);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var report = await response.Content.ReadFromJsonAsync<EmbedContentDTO>();
                    return new()
                    {
                        IsSuccess = true,
                        Value = report,
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
    }
}
