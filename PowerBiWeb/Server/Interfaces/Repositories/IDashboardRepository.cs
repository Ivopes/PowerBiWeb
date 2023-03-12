using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<ProjectDashboard?> GetByIdAsync(Guid dashboardId);
        Task<string> UpdateDashboardsAsync(int projectId);
        Task<string> EditDashboard(ProjectDashboard dashboard);
    }
}