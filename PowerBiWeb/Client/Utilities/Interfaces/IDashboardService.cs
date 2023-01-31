using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IDashboardService
    {
        Task<HttpResponse<EmbedReportDTO?>> GetDashboardAsync(int projectId, Guid dashboardId);
        Task<HttpResponse<EmbedReportDTO?>> GetDashboardAsync(int projectId, Guid dashboardId, CancellationToken ct);
    }
}