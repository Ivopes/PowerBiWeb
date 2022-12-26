using Microsoft.PowerBI.Api.Models;

namespace PowerBiWeb.Shared.Project
{
    public class EmbedReportDTO
    {
        // Id of Power BI report to be embedded
        public Guid ReportId { get; set; }

        // Name of the report
        public string ReportName { get; set; } = string.Empty;

        // Embed URL for the Power BI report
        public string EmbedUrl { get; set; } = string.Empty;
        // Embed Token for the Power BI report
        public string EmbedToken { get; set; } = string.Empty;

    }
}
