using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PowerBiContext _dbContext;
        private readonly IMetricsSaverRepository _metricsSaverRepository;
        public DashboardRepository(PowerBiContext dbContext, IMetricsSaverRepository metricsSaverRepository)
        {
            _dbContext = dbContext;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<ProjectDashboard?> GetByIdAsync(int projectId, Guid dashboardId)
        {
            var entity = await _dbContext.ProjectDashboards.Include(d => d.Project).SingleAsync(d => d.PowerBiId == dashboardId && d.Project.Id == projectId);

            return entity;
        }
        public async Task<string> UpdateReportsAsync(int projectId)
        {
            return await _metricsSaverRepository.UpdateDashboardsAsync(projectId);
        }
    }
}
