using MetricsAPI.Models;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to load data from metrics server
    /// </summary>
    public interface IMetricsApiLoaderRepository
    {
        Task<MetricPortion?> GetMetric(string projectName, string metricName, bool isTotal);
        Task<List<MetricPortion>> GetMetricAll(string projectName, bool isTotal);
    }
}
