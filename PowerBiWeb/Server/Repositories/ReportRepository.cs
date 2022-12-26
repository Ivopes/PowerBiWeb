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
        private readonly PowerBiContext _dbContext;
        private readonly IMetricsSaverRepository _metricsSaverRepository;
        public ReportRepository(PowerBiContext dbContext, IMetricsSaverRepository metricsSaverRepository)
        {
            _dbContext = dbContext;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public Task<EmbedParams> GetAsync(int projectId)
        {
            throw new NotImplementedException();
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
    }
}
