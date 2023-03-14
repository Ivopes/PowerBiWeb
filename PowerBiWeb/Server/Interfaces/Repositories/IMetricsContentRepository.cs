using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Models.Metrics;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    /// <summary>
    /// Interface used to upload metrics to metric server to create graphs (for example PowerBI)
    /// </summary>
    public interface IMetricsContentRepository
    {
        Task<DashboardDTO> GetEmbededDashboardAsync(Guid dashboardId);
        Task<ReportDTO> GetEmbededReportAsync(Guid reportId);
        Task<string> UpdateDashboardsAsync(int projectId);
        Task<string> AddDashboardsAsync(int projectId, ProjectDashboard dashboard);
        Task<string> UpdateReportsAsync(int projectId);
        Task<string> AddReportsAsync(int projectId, ProjectReport report);
        Task<Dataset?> CreateDatasetFromDefinition(MetricDefinition definition);
        Task<bool> AddRowsToDataset(PBIDataset dataset, MetricData data);
        Task<Report?> CloneReportAsync(Guid reportId, string reportNewName);
        Task<bool> RebindReportAsync(Guid reportId, Guid datasetId);
        Task<Stream?> GetExportedReportAsync(Guid reportId);
        Task<Stream?> GetDownloadedReportAsync(Guid reportId);
    }
}
