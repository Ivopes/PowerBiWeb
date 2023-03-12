using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IReportService
    {
        Task<HttpResponse<EmbedContentDTO?>> GetReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<EmbedContentDTO?>> GetReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse<EmbedContentDTO[]?>> GetReportsAsync(int projectId);
        Task<HttpResponse<EmbedContentDTO[]?>> GetReportsAsync(int projectId, CancellationToken ct);
        Task<HttpResponse> CloneReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse> RebindReportAsync(int projectId, Guid reportId, Guid datasetId, CancellationToken ct);
    }
}