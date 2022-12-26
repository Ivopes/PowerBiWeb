using MetricsAPI.Models;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to upload metrics to metric server to create graphs (for example PowerBI)
    /// </summary>
    public interface IMetricsSaverRepository
    {
        Task<EmbedReportDTO> GetEmbededAsync(Guid reportId);
        Task<string> UpdateReportsAsync(int projectId);
        Task UploadMetric(Project project, MetricPortion metric);
        Task UploadMetric(Project project, List<MetricPortion> metrics);
    }
}
