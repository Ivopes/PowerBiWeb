using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IReportService
    {
        Task<string> CloneReportAsync(int projectId, Guid reportId);
        Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateReportsAsync(int projectId);
    }
}