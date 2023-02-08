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
    public interface IMetricsContentRepository
    {
        Task<EmbedContentDTO> GetEmbededDashboardAsync(Guid dashboardId);
        Task<EmbedContentDTO> GetEmbededReportAsync(Guid reportId);
        Task<string> UpdateDashboardsAsync(int dashboardId);
        Task<string> AddDashboardsAsync(int projectId, ProjectDashboard dashboard);
        Task<string> UpdateReportsAsync(int reportId);
        Task<string> AddReportsAsync(int projectId, ProjectReport report);
        Task UploadMetric(PBIDataset dataset, MetricPortion metric);
        Task UploadMetric(PBIDataset dataset, List<MetricPortion> metrics);
    }
}
