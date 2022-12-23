using MetricsAPI.Models;
using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Utilities.Constants;
using System.IO;

namespace PowerBiWeb.Server.Repositories
{
    public class MetricsApiRepository : IMetricsApiLoaderRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string IncPath = "/metrics/{0}/inc/{1}";
        private const string TotalPath = "/metrics/{0}/total/{1}";

        public MetricsApiRepository(IHttpClientFactory factory)
        {
            _clientFactory = factory;
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

            var httpResult = await httpClient.GetAsync(path);

            if (httpResult.IsSuccessStatusCode)
            {
                result = await httpResult.Content.ReadFromJsonAsync<MetricPortion>();
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

            var httpResult = await httpClient.GetAsync(path);

            if (httpResult.IsSuccessStatusCode)
            {
                result = (await httpResult.Content.ReadFromJsonAsync<List<MetricPortion>>())!;
            }
            return result;
        }

        public async Task<List<MetricPortion>> GetMetricLatestAll(string projectName)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            List<MetricPortion> result = Enumerable.Empty<MetricPortion>().ToList();

            string path = $"/metrics/{projectName}/latest";

            var httpResult = await httpClient.GetAsync(path);

            if (httpResult.IsSuccessStatusCode)
            {
                result = (await httpResult.Content.ReadFromJsonAsync<List<MetricPortion>>())!;
            }
            return result;
        }

        public async Task<MetricPortion?> GetMetricLatest(string projectName, string metricName)
        {
            var httpClient = _clientFactory.CreateClient(HttpClientTypes.MetricsApi);

            MetricPortion? result = null;

            string path = $"/metrics/{projectName}/latest/{metricName}";

            var httpResult = await httpClient.GetAsync(path);

            if (httpResult.IsSuccessStatusCode)
            {
                result = await httpResult.Content.ReadFromJsonAsync<MetricPortion>();
            }
            return result;
        }
    }
}
