using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Project;
using System.IO;
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
        public async Task<HttpResponse<ReportDTO[]?>> GetReportsAsync(int projectId)
        {
            return await GetReportsAsync(projectId, CancellationToken.None);
        }
        public async Task<HttpResponse<ReportDTO[]?>> GetReportsAsync(int projectId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}", ct);
            if (response.IsSuccessStatusCode)
            {
                var reports = await response.Content.ReadFromJsonAsync<ReportDTO[]>();
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
        public async Task<HttpResponse<ReportDTO?>> GetReportAsync(int projectId, Guid reportId)
        {
            return await GetReportAsync(projectId, reportId, CancellationToken.None);
        }
        public async Task<HttpResponse<ReportDTO?>> GetReportAsync(int projectId, Guid reportId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}/{reportId}", ct);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var report = await response.Content.ReadFromJsonAsync<ReportDTO>();
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
        public async Task<HttpResponse> CloneReportAsync(int projectId, Guid reportId, CancellationToken ct)
        {
            var response = await _httpClient.PostAsync($"api/reports/clone/{projectId}/{reportId}", null, ct);
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
                    _logger.LogError(ex, "report could not been cloned");
                }
            }

            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }

        public async Task<HttpResponse> RebindReportAsync(int projectId, Guid reportId, Guid datasetId, CancellationToken ct)
        {
            var response = await _httpClient.PostAsync($"api/reports/rebind/{projectId}/{reportId}/{datasetId}", null, ct);
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
                    _logger.LogError(ex, "report could not been rebinded");
                }
            }

            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }

        public async Task<HttpResponse> UpdateReportSettingsAsync(ReportDTO report)
        {
            return await UpdateReportSettingsAsync(report, CancellationToken.None);
        }

        public async Task<HttpResponse> UpdateReportSettingsAsync(ReportDTO report, CancellationToken ct)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/reports", report, ct);
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
                    _logger.LogError(ex, "report could not been changed");
                }
            }

            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }

        public async Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId)
        {
            return await ExportReportAsync(projectId, reportId, CancellationToken.None);
        }

        public async Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"api/reports/export/{projectId}/{reportId}", ct);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return new()
                    {
                        IsSuccess = true,
                        Value = stream,
                        ErrorMessage = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "report could not been downloaded");
                }
            }

            return new()
            {
                ErrorMessage = "Could not download a file",
            };
        }
    }
}
