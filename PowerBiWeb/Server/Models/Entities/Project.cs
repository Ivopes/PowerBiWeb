using System.ComponentModel.DataAnnotations.Schema;

namespace PowerBiWeb.Server.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [NotMapped]
        public bool DownloadContent { get; set; } // only needed during creation

        // Other entities
        public ICollection<AppUserProject> AppUserProjects { get; set; } = new HashSet<AppUserProject>();
        public ICollection<ProjectReport> ProjectReports { get; set; } = new HashSet<ProjectReport>();
        public ICollection<ProjectDashboard> ProjectDashboards { get; set; } = new HashSet<ProjectDashboard>();
    }
}
