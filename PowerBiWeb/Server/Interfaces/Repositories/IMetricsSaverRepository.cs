using MetricsAPI.Models;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to upload metrics to metric server to create graphs (for example PowerBI)
    /// </summary>
    public interface IMetricsSaverRepository
    {
        Task UploadMetric(MetricPortion metric);
        Task UploadMetric(List<MetricPortion> metric);
    }
}
