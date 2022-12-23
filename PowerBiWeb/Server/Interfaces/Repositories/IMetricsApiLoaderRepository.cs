using MetricsAPI.Models;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to load data from metrics server
    /// </summary>
    public interface IMetricsApiLoaderRepository
    {
        Task<MetricPortion?> GetMetric(string projectName, string metricName, bool isTotal);
        Task<List<MetricPortion>> GetMetricAllAsync(string projectName, bool isTotal);
        /// <summary>
        /// Download all latest increment metric for specific project
        /// </summary>
        Task<List<MetricPortion>> GetMetricLatestAll(string projectName);
        /// <summary>
        /// Download specific latest increment metric for specific project
        /// </summary>
        Task<MetricPortion?> GetMetricLatest(string projectName, string metricName);
    }
}
