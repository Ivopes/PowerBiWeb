namespace PowerBiWeb.Server.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string MetricFilesName { get; set; } = string.Empty;

        // Other entities
        public ICollection<AppUserProject> AppUserProject { get; set; } = new HashSet<AppUserProject>();
    }
}
