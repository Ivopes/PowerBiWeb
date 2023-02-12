namespace PowerBiWeb.Server.Models.Entities
{
    public class PBIDataset
    {
        public int Id { get; set; }
        public string MetricFilesId { get; set; } = string.Empty;
        public Guid PowerBiId { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<string> ColumnNames { get; set; } = Array.Empty<string>().ToList();
        public List<string> ColumnTypes { get; set; } = Array.Empty<string>().ToList();
        public List<string> Measures { get; set; } = Array.Empty<string>().ToList();
        public List<string> MeasureDefinitions { get; set; } = Array.Empty<string>().ToList();

        // Other entities
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}