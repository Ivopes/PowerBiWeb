using PowerBiWeb.Client.Pages.Projects;
using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Datasets;
using PowerBiWeb.Shared.Project;
using System.Net.Http.Json;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class DatasetService : IDatasetService
    {
        private HttpClient _httpClient;
        private ILogger<DatasetService> _logger;

        public DatasetService(HttpClient httpClient, ILogger<DatasetService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponse<List<DatasetDTO>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"api/datasets/");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var datasets = await response.Content.ReadFromJsonAsync<List<DatasetDTO>>();
                    return new()
                    {
                        IsSuccess = true,
                        Value = datasets,
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

        public async Task<HttpResponse<DatasetDTO>> GetDatasetDetailAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/datasets/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var datasets = await response.Content.ReadFromJsonAsync<DatasetDTO>();
                    return new()
                    {
                        IsSuccess = true,
                        Value = datasets,
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
