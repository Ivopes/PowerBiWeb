using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<ProjectDashboard?> GetByIdAsync(int projectId, Guid dashboardId);
        Task<string> UpdateDashboardsAsync(int projectId);
    }
}