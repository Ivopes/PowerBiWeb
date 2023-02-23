using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IReportService
    {
        Task<string> CloneReportAsync(int projectId, Guid reportId);
        Task<EmbedContentDTO?> GetByIdAsync(int projectId, Guid reportId);
        Task<string> UpdateReportsAsync(int projectId);
        /// <summary>
        /// Rebind report to another dataset with the same structure
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="reportId"></param>
        /// <param name="datasetId"></param>
        /// <returns></returns>
        Task<string> RebindReportAsync(int projectId, Guid reportId, Guid datasetId);
    }
}