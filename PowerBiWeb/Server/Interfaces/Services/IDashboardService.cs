using PowerBiWeb.Shared.Projects;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardDTO?> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateDashboardsAsync(int projectId);
        Task<string> UpdateDashboardSettingsAsync(DashboardDTO dashboard);
    }
}