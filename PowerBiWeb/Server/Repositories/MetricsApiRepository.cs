using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Metrics;
using PowerBiWeb.Server.Utilities.Constants;

namespace PowerBiWeb.Server.Repositories
{
    public class MetricsApiRepository : IMetricsApiLoaderRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string IncPath = "/metrics/{0}/inc/{1}";
        private const string TotalPath = "/metrics/{0}/total/{1}";
        private const string DefinitionPath = "/metrics/definition/{0}";

        private const string IncPathNew = "/new/metrics/inc/{0}";
        private const string TotalPathNew = "/new/metrics/total/{0}";
        private const string DefinitionPathNew = "/new/metrics/definition/{0}";
        private readonly ILogger<MetricsApiRepository> _logger;

        public MetricsApiRepository(IHttpClientFactory factory, ILogger<MetricsApiRepository> logger)
        {
            _clientFactory = factory;
            _logger = logger;
        }
        public async Task<MetricPortion?> GetMetric(string projectName, string metricName, bool isTotal)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricPortion? result = null;

            string path;
            if (isTotal)
                path = string.Format(TotalPath, projectName, metricName);
            else
                path = string.Format(IncPath, projectName, metricName);

            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = await httpResult.Content.ReadFromJsonAsync<MetricPortion>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics for project: {0}", projectName);
            }
            return result;
        }
        public async Task<List<MetricPortion>> GetMetricAllAsync(string projectName, bool isTotal)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            List<MetricPortion> result = Enumerable.Empty<MetricPortion>().ToList();

            string path;
            if (isTotal)
                path = string.Format(TotalPath, projectName, string.Empty);
            else
                path = string.Format(IncPath, projectName, string.Empty);

            try
            {
                var httpResult = await httpClient.GetAsync(path);
                if (httpResult.IsSuccessStatusCode)
                {
                    result = (await httpResult.Content.ReadFromJsonAsync<List<MetricPortion>>())!;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics for project: {0}", projectName);
            }
            return result;
        }
        public async Task<List<MetricPortion>> GetMetricLatestAll(string projectName)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            List<MetricPortion> result = Enumerable.Empty<MetricPortion>().ToList();

            string path = $"/metrics/{projectName}/latest";
            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = (await httpResult.Content.ReadFromJsonAsync<List<MetricPortion>>())!;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics for project: {0}", projectName);
            }
            return result;
        }
        public async Task<MetricPortion?> GetMetricLatest(string projectName, string metricName)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricPortion? result = null;

            string path = $"/metrics/{projectName}/latest/{metricName}";
            try
            {
                var httpResult = await httpClient.GetAsync(path);

                if (httpResult.IsSuccessStatusCode)
                {
                    result = await httpResult.Content.ReadFromJsonAsync<MetricPortion>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not load metrics for project: {0}", projectName);
            }
            return result;
        }
        public async Task<MetricDefinition?> GetMetricDefinition(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricDefinition? result = null;

            string path = string.Format(DefinitionPathNew, datasetId);
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
        public async Task<MetricData?> GetMetricTotalNew(string datasetId)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricData? result = null;

            string path = string.Format(TotalPathNew, datasetId);
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
