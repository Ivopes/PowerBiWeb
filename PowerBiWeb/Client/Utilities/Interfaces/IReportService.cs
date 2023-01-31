using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IReportService
    {
        Task<HttpResponse<EmbedReportDTO?>> GetReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<EmbedReportDTO?>> GetReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse<EmbedReportDTO[]?>> GetReportsAsync(int projectId);
        Task<HttpResponse<EmbedReportDTO[]?>> GetReportsAsync(int projectId, CancellationToken ct);
    }
}