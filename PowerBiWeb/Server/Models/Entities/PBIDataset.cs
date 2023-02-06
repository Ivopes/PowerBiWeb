namespace PowerBiWeb.Server.Models.Entities
{
    public class PBIDataset
    {
        public int Id { get; set; }
        public string MetricFilesId { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        // Other entities
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }
}