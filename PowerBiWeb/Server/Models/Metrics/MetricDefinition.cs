namespace PowerBiWeb.Server.Models.Metrics
{
    public class MetricDefinition
    {
        public string Name { get; set; } = string.Empty;
        public List<string> ColumnNames { get; set; } = Array.Empty<string>().ToList();
        public List<string> ColumnTypes { get; set; } = Array.Empty<string>().ToList();
        public List<string> Measures { get; set; } = Array.Empty<string>().ToList();
        public List<string> MeasureDefinitions { get; set; } = Array.Empty<string>().ToList();
    }
}
