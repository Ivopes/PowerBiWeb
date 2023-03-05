using Microsoft.CodeAnalysis;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
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

        public async Task<string> CloneReportAsync(int projectId, Guid reportId)
        {
            var report = await _reportRepository.GetByGuidAsync(reportId);

            if (report is null)
            {
                _logger.LogError("Report with id: {0} was not found", reportId);
                return "Report not found";
            }

            var project = report.Projects.SingleOrDefault(p => p.Id == projectId);

            if (project is null)
            {
                _logger.LogError("Report with id: {0} is not part of project: {1}", reportId, projectId);
                return "Report is not part of this project";
            }

            var createdR = await _metricsSaverRepository.CloneReportAsync(reportId, report.PowerBIName + " - Cloned");

            if (createdR is null)
            {
                _logger.LogError("Could not clone report {0}", reportId);
                return "Could not clone project";
            }

            var newR = new ProjectReport
            {
                Name = report.Name + " - Cloned",
                PowerBiId = createdR.Id,
                PowerBIName = createdR.Name,
                WorkspaceId = report.WorkspaceId,
                Projects = new List<Models.Entities.Project> { project },
                Dataset = report.Dataset,
                DatasetId = report.DatasetId
            };

            await _reportRepository.AddReportAsync(newR);

            return string.Empty;
        }
        public async Task<string> RebindReportAsync(int projectId, Guid reportId, Guid datasetId)
        {
            if (await _metricsSaverRepository.RebindReportAsync(reportId, datasetId))
            {
                return string.Empty;
            }

            return "Could not rebind report. Check the dataset ID and dataset format";
        }
        public async Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid reportId)
        {
            // Check if report belongs to project
            var report = await _reportRepository.GetByGuidAsync(reportId);

            if (report == null || report.Projects.All(p => p.Id != projectId))
            {
                _logger.LogError("Report with id: {0} for project id: {1} was not found", reportId, projectId);
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
