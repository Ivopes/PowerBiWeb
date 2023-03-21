using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PowerBiContext _dbContext;
        private readonly IMetricsContentRepository _metricsSaverRepository;
        public DashboardRepository(PowerBiContext dbContext, IMetricsContentRepository metricsSaverRepository)
        {
            _dbContext = dbContext;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<ProjectDashboard?> GetByIdAsync(Guid dashboardId)
        {
            var entity = await _dbContext.ProjectDashboards.Include(d => d.Projects).SingleOrDefaultAsync(d => d.PowerBiId == dashboardId);

            return entity;
        }
        public async Task<string> UpdateDashboardsAsync(int projectId)
        {
            return await _metricsSaverRepository.UpdateDashboardsAsync(projectId);
        }

        public async Task<string> EditDashboard(ProjectDashboard dashboard)
        {
            var entity = await _dbContext.ProjectDashboards.FindAsync(dashboard.PowerBiId);

            if (entity is null) return "Dashboard not found";
            
            entity.Name = dashboard.Name;

            await _dbContext.SaveChangesAsync();
            
            return string.Empty;
        }
    }
}
