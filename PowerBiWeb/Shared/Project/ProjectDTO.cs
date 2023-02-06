using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Shared.Project
{
    public class ProjectDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ConnectedMetricsIds { get; set; } = string.Empty;
        [Required]
        public string PowerBiPrefix { get; set; } = string.Empty;
        public bool DownloadContent { get; set; }
        public ICollection<UserDTO> Users { get; set; } = Array.Empty<UserDTO>();
        public ICollection<EmbedReportDTO> Reports { get; set; } = Array.Empty<EmbedReportDTO>();
        public ICollection<EmbedReportDTO> Dashboards { get; set; } = Array.Empty<EmbedReportDTO>();
    }
}
