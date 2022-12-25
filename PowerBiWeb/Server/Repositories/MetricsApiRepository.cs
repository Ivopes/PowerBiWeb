using MetricsAPI.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Utilities.Constants;

namespace PowerBiWeb.Server.Repositories
{
    public class MetricsApiRepository : IMetricsApiLoaderRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string IncPath = "/metrics/{0}/inc/{1}";
        private const string TotalPath = "/metrics/{0}/total/{1}";
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
    }
}
