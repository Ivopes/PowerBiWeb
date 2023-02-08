using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IDashboardService
    {
        Task<HttpResponse<EmbedContentDTO?>> GetDashboardAsync(int projectId, Guid dashboardId);
        Task<HttpResponse<EmbedContentDTO?>> GetDashboardAsync(int projectId, Guid dashboardId, CancellationToken ct);
    }
}