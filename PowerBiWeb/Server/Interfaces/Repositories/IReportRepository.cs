using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<ProjectReport?> GetByGuidAsync(Guid reportGuid);
        Task<string> UpdateReportsAsync(int projectId);
        Task<ProjectReport?> AddReportAsync(ProjectReport report);
    }
}