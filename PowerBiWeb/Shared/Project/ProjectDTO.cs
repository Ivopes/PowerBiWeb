using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Shared.Project
{
    public class ProjectDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool DownloadContent { get; set; }
        public ICollection<UserDTO> Users { get; set; } = Array.Empty<UserDTO>();
        public ICollection<EmbedContentDTO> Reports { get; set; } = Array.Empty<EmbedContentDTO>();
        public ICollection<EmbedContentDTO> Dashboards { get; set; } = Array.Empty<EmbedContentDTO>();
    }
}
