namespace PowerBiWeb.Server.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string MetricFilesName { get; set; } = string.Empty;
        public string PowerBiPrefix { get; set; } = string.Empty;
        public bool CreateDatasets { get; set; }
        public bool DownloadContent { get; set; }

        // Other entities
        public ICollection<AppUserProject> AppUserProjects { get; set; } = new HashSet<AppUserProject>();
        public ICollection<ProjectReport> ProjectReports { get; set; } = new HashSet<ProjectReport>();
        public ICollection<ProjectDashboard> ProjectDashboards { get; set; } = new HashSet<ProjectDashboard>();
    }
}
