using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IDashboardService
    {
        Task<HttpResponse<DashboardDTO?>> GetDashboardAsync(int projectId, Guid dashboardId);
        Task<HttpResponse<DashboardDTO?>> GetDashboardAsync(int projectId, Guid dashboardId, CancellationToken ct);
        Task<HttpResponse> UpdateDashboardSettingsAsync(DashboardDTO dashboard);
        Task<HttpResponse> UpdateDashboardSettingsAsync(DashboardDTO dashboard, CancellationToken ct);
    }
}