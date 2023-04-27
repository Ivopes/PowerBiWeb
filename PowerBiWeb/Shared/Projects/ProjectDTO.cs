using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Shared.Projects
{
    public class ProjectDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<UserDTO> Users { get; set; } = Array.Empty<UserDTO>();
        public ICollection<ReportDTO> Reports { get; set; } = Array.Empty<ReportDTO>();
        public ICollection<DashboardDTO> Dashboards { get; set; } = Array.Empty<DashboardDTO>();
    }
}
