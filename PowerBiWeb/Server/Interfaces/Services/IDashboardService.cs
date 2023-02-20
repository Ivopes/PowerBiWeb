using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateDashboardsAsync(int projectId);
    }
}