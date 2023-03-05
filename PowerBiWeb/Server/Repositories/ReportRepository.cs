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
        private readonly IMetricsContentRepository _metricsSaverRepository;
        public ReportRepository(PowerBiContext dbContext, IMetricsContentRepository metricsSaverRepository)
        {
            _dbContext = dbContext;
            _metricsSaverRepository = metricsSaverRepository;
        }

        public async Task<ProjectReport?> AddReportAsync(ProjectReport report)
        {
            var result = await _dbContext.ProjectReports.AddAsync(report);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }
        public async Task<ProjectReport?> GetByGuidAsync(Guid reportGuid)
        {
            var entity = await _dbContext.ProjectReports.Include(r => r.Projects).Include(r => r.Dataset).SingleOrDefaultAsync(r => r.PowerBiId == reportGuid);

            return entity;
        }
        public async Task<string> UpdateReportsAsync(int projectId)
        {
            return await _metricsSaverRepository.UpdateReportsAsync(projectId);
        }
    }
}
