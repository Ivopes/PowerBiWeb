using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Server.Models.Entities
{
    public class ProjectReports
    {
        public Guid PowerBiId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Project Project { get; set; } = new();
        public int ProjectId { get; set; }

    }
}
