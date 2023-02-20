using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Repositories;
using PowerBiWeb.Server.Utilities.Exceptions;
using PowerBiWeb.Shared.Project;

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
        public async Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid dashboardId)
        {
            // Check if dashboard belongs to project
            var dashboard = await _dashboardRepository.GetByIdAsync(projectId, dashboardId);

            if (dashboard is null)
            {
                _logger.LogError("Dashboard with id: {0} for project id: {1} was not found", dashboardId, projectId);
                return null;
            }

            var result = await _metricsSaverRepository.GetEmbededDashboardAsync(dashboardId);
            result.Name = dashboard.Name;

            return result;
        }
        public async Task<string> UpdateDashboardsAsync(int projectId)
        {
            return await _dashboardRepository.UpdateDashboardsAsync(projectId);
        }
    }
}
