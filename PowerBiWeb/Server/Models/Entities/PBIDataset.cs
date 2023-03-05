using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Server.Models.Entities
{
    public class PBIDataset
    {
        [Key]
        public int Id { get; set; }
        public string MetricFilesId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid PowerBiId { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<string> ColumnNames { get; set; } = Array.Empty<string>().ToList();
        public List<string> ColumnTypes { get; set; } = Array.Empty<string>().ToList();
        public List<string> Measures { get; set; } = Array.Empty<string>().ToList();
        public List<string> MeasureDefinitions { get; set; } = Array.Empty<string>().ToList();
        public ICollection<ProjectReport> Reports { get; set; } = new HashSet<ProjectReport>();

    }
}