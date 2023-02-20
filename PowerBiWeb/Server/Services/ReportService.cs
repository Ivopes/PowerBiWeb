using Microsoft.CodeAnalysis;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Utilities.Exceptions;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportService> _logger;
        private readonly IMetricsContentRepository _metricsSaverRepository;
        public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger, IMetricsContentRepository metricsSaverRepository)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<EmbedParams> GetAsync(int projectId)
        {
            return await _reportRepository.GetAsync(projectId);
        }
        public async Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid reportId)
        {
            // Check if report belongs to project
            var report = await _reportRepository.GetByIdAsync(reportId);

            if (report == null || report.Projects.All(p => p.Id != projectId))
            {
                _logger.LogError("Report with id: {0} for project id: {1} wwas not found", reportId, projectId);
                return null;
            }

            var result = await _metricsSaverRepository.GetEmbededReportAsync(reportId);
            result.Name = report.Name;

            return result;
        }

        public async Task<string> UpdateReportsAsync(int projectId)
        {
            return await _reportRepository.UpdateReportsAsync(projectId);
        }
    }
}
