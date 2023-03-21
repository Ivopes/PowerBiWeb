using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Repositories;
using PowerBiWeb.Server.Utilities.Exceptions;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared.Projects;

namespace PowerBiWeb.Server.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<ReportService> _logger;
        private readonly IMetricsContentRepository _metricsSaverRepository;
        public DashboardService(IDashboardRepository dashboardRepository, ILogger<ReportService> logger, IMetricsContentRepository metricsSaverRepository)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<DashboardDTO?> GetByIdAsync(int projectId, Guid dashboardId)
        {
            // Check if dashboard belongs to project
            var dashboard = await _dashboardRepository.GetByIdAsync(dashboardId);

            if (dashboard is null)
            {
                _logger.LogError("Dashboard with id: {0}", dashboardId);
                return null;
            }
            
            var project = dashboard.Projects.SingleOrDefault(p => p.Id == projectId);

            if (project is null)
            {
                _logger.LogError("Dashboard with id: {0} is not part of project: {1}", dashboardId, projectId);
                return null;
            }

            var result = await _metricsSaverRepository.GetEmbededDashboardAsync(dashboardId);
            result.Name = dashboard.Name;

            result.Projects = new()
            {
                dashboard.Projects.Single(p => p.Id == projectId).ToDTO()
            };
            
            return result;
        }
        public async Task<string> UpdateDashboardsAsync(int projectId)
        {
            return await _dashboardRepository.UpdateDashboardsAsync(projectId);
        }

        public async Task<string> UpdateDashboardSettingsAsync(DashboardDTO dashboard)
        {
            var d = dashboard.ToBO();

            return await _dashboardRepository.EditDashboard(d);
        }
    }
}
