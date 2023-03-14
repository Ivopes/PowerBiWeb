using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IReportService
    {
        Task<HttpResponse<ReportDTO?>> GetReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<ReportDTO?>> GetReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<Stream>> ExportReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse<ReportDTO[]?>> GetReportsAsync(int projectId);
        Task<HttpResponse<ReportDTO[]?>> GetReportsAsync(int projectId, CancellationToken ct);
        Task<HttpResponse> CloneReportAsync(int projectId, Guid reportId, CancellationToken ct);
        Task<HttpResponse> RebindReportAsync(int projectId, Guid reportId, Guid datasetId, CancellationToken ct);
        Task<HttpResponse> UpdateReportSettingsAsync(ReportDTO report);
        Task<HttpResponse> UpdateReportSettingsAsync(ReportDTO report, CancellationToken ct);
        Task<HttpResponse<Stream>> DownloadReportAsync(int projectId, Guid reportId);
        Task<HttpResponse<Stream>> DownloadReportAsync(int projectId, Guid reportId, CancellationToken ct);
    }
}