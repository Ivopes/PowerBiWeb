using PowerBiWeb.Server.Models.Metrics;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to load data from metrics server
    /// </summary>
    public interface IMetricsApiLoaderRepository
    {
        Task<MetricDefinition?> GetMetricDefinition(string datasetId);
        Task<MetricData?> GetMetricTotal(string datasetId);
        /// <summary>
        /// Download specific latest increment metric for specific project
        /// </summary>
        Task<MetricData?> GetMetricIncrement(string datasetId);
    }
}
