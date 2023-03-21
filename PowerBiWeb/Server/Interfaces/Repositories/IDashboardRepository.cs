using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<ProjectDashboard?> GetByIdAsync(Guid dashboardId);
        Task<string> UpdateDashboardsAsync(int projectId);
        Task<string> EditDashboard(ProjectDashboard dashboard);
    }
}