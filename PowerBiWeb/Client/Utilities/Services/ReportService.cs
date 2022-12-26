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
        public async Task<HttpResponse<EmbedReportDTO[]?>> GetReportsAsync(int projectId)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}");
            if (response.IsSuccessStatusCode)
            {
                var reports = await response.Content.ReadFromJsonAsync<EmbedReportDTO[]>();
                return new()
                {
                    Success = true,
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
        public async Task<HttpResponse<EmbedReportDTO?>> GetReportAsync(int projectId, Guid reportId)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}/{reportId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var report = await response.Content.ReadFromJsonAsync<EmbedReportDTO>();
                    return new()
                    {
                        Success = true,
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
