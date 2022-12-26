using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<EmbedParams> GetAsync();
        Task<EmbedParams> GetAsync(int projectId);
        Task<ProjectReport?> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateReportsAsync(int projectId);
    }
}