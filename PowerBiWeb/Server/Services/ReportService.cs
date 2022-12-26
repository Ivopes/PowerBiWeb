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
        private readonly IMetricsSaverRepository _metricsSaverRepository;
        public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger, IMetricsSaverRepository metricsSaverRepository)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<EmbedParams> GetAsync(int projectId)
        {
            return await _reportRepository.GetAsync(projectId);
        }
        public async Task<EmbedReportDTO> GetByIdAsync(int projectId, Guid reportId)
        {
            // Check if report belongs to project
            var report = await _reportRepository.GetByIdAsync(projectId, reportId);

            if (report == null)
            {
                _logger.LogError("Report with id: {0} for project id: {1} wwas not found", reportId, projectId);
                throw new MessageException
                {
                    Message = "Report was not found"
                };
            }

            var result = await _metricsSaverRepository.GetEmbededAsync(reportId);

            return result;
        }

        public async Task<string> UpdateReportsAsync(int projectId)
        {
            return await _reportRepository.UpdateReportsAsync(projectId);
        }
    }
}
