using MetricsAPI.Models;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to upload metrics to metric server to create graphs (for example PowerBI)
    /// </summary>
    public interface IMetricsSaverRepository
    {
        Task UploadMetric(Project project, MetricPortion metric);
        Task UploadMetric(Project project, List<MetricPortion> metrics);
    }
}
