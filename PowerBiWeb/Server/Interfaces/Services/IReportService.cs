using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IReportService
    {
        Task<EmbedParams> GetAsync(int projectId);
        Task<EmbedReportDTO> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateReportsAsync(int projectId);
    }
}