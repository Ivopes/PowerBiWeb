using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Server.Models.Entities
{
    public class ProjectReport
    {
        [Key]
        public Guid PowerBiId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PowerBIName { get; set; } = string.Empty;
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();
        public PBIDataset Dataset { get; set; }
    }
}
