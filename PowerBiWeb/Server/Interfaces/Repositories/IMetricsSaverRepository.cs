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
        Task<EmbedReportDTO> GetEmbededDashboardAsync(Guid dashboardId);
        Task<EmbedReportDTO> GetEmbededReportAsync(Guid reportId);
        Task<string> UpdateDashboardsAsync(int dashboardId);
        Task<string> UpdateReportsAsync(int reportId);
        Task UploadMetric(PBIDataset dataset, MetricPortion metric);
        Task UploadMetric(PBIDataset dataset, List<MetricPortion> metrics);
    }
}
