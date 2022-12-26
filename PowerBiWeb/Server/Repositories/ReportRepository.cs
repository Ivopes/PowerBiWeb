using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Server.Utilities.PowerBI;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;
using System.Runtime.InteropServices;

namespace PowerBiWeb.Server.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AadService _aadService;
        private readonly PowerBiContext _dbContext;
        private readonly IMetricsSaverRepository _metricsSaverRepository;
        public ReportRepository(AadService aadService, PowerBiContext dbContext, IMetricsSaverRepository metricsSaverRepository)
        {
            _aadService = aadService;
            _dbContext = dbContext;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public Task<EmbedParams> GetAsync(int projectId)
        {
            throw new NotImplementedException();
        }
        public Task<EmbedParams> GetAsync()
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            Guid workspaceId = Guid.Parse(_aadService.WorkspaceId);

            var reports = pbiClient.Reports.GetReports(workspaceId);

            var reportGuids = new List<Guid>();

            reportGuids.Add(reports.Value[0].Id);

            Guid reportId = reportGuids[0];

            var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);


            //  Check if dataset is present for the corresponding report
            //  If isRDLReport is true then it is a RDL Report 
            var isRDLReport = String.IsNullOrEmpty(pbiReport.DatasetId);

            EmbedToken embedToken = null;

            // Generate embed token for RDL report if dataset is not present
            if (isRDLReport)
            {
                // Get Embed token for RDL Report
                //embedToken = GetEmbedTokenForRDLReport(workspaceId, reportId);
            }
            else
            {
                // Create list of datasets
                var datasetIds = new List<Guid>();

                // Add dataset associated to the report
                datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                // Get Embed token multiple resources
                embedToken = GetEmbedToken(reportId, datasetIds, workspaceId);
            }

            // Add report data for embedding

            var embedReports = new List<EmbedReportDTO>()
            {
                new EmbedReportDTO
                {
                    ReportId = pbiReport.Id,
                    ReportName = pbiReport.Name,
                    EmbedUrl = pbiReport.EmbedUrl
                }
            };

            // Capture embed params
            var embedParams = new EmbedParams
            {
                EmbedReport = embedReports,
                Type = "report",
                EmbedToken = embedToken
            };

            return Task.FromResult(embedParams);
        }

        public async Task<ProjectReport?> GetByIdAsync(int projectId, Guid reportId)
        {
            var entity = await _dbContext.ProjectReports.Include(r => r.Project).SingleAsync(r => r.PowerBiId == reportId && r.Project.Id == projectId);

            return entity;
        }

        public async Task<string> UpdateReportsAsync(int projectId)
        {
            return await _metricsSaverRepository.UpdateReportsAsync(projectId);
        }

        /// <summary>
        /// Get Embed token for single report, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        private EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
            );

            // Generate Embed token
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }
    }
}
