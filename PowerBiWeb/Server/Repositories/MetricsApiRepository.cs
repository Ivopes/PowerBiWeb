using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Metrics;
using PowerBiWeb.Server.Utilities.Constants;

namespace PowerBiWeb.Server.Repositories
{
    public class MetricsApiRepository : IMetricsApiLoaderRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        private const string IncPath = "/metrics/inc/{0}";
        private const string TotalPath = "/metrics/total/{0}";
        private const string DefinitionPath = "/metrics/definition/{0}";
        private readonly ILogger<MetricsApiRepository> _logger;

        public MetricsApiRepository(IHttpClientFactory factory, ILogger<MetricsApiRepository> logger)
        {
            _clientFactory = factory;
            _logger = logger;
        }
        public async Task<MetricDefinition?> GetMetricDefinition(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricDefinition? result = null;

            string path = string.Format(DefinitionPath, datasetId);
            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = await httpResult.Content.ReadFromJsonAsync<MetricDefinition>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metric definition with id: {0}", datasetId);
            }

            return result;
        }
        public async Task<MetricData?> GetMetricTotal(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricData? result = null;

            string path = string.Format(TotalPath, datasetId);
            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = await httpResult.Content.ReadFromJsonAsync<MetricData>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics with id: {0}", datasetId);
            }
            return result;
        }
        public async Task<MetricData?> GetMetricIncrement(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricData? result = null;

            string path = string.Format(IncPath, datasetId);
            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = await httpResult.Content.ReadFromJsonAsync<MetricData>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics with id: {0}", datasetId);
            }
            return result;
        }
    }
}
