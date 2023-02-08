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
        public async Task<EmbedContentDTO> GetByIdAsync(int projectId, Guid dashboardId)
        {
            // Check if dashboard belongs to project
            var report = await _dashboardRepository.GetByIdAsync(projectId, dashboardId);

            if (report == null)
            {
                _logger.LogError("Dashboard with id: {0} for project id: {1} was not found", dashboardId, projectId);
                throw new MessageException
                {
                    ExcptMessage = "Dashboard was not found"
                };
            }

            var result = await _metricsSaverRepository.GetEmbededDashboardAsync(dashboardId);

            return result;
        }

        public async Task<string> UpdateDashboardsAsync(int projectId)
        {
            return await _dashboardRepository.UpdateDashboardsAsync(projectId);
        }
    }
}
