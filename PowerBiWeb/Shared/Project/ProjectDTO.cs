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
        public string MetricName { get; set; } = string.Empty;
        public ICollection<UserDTO> Users { get; set; } = Array.Empty<UserDTO>();
    }
}
