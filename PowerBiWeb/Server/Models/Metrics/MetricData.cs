namespace PowerBiWeb.Server.Models.Metrics
{
    public class MetricData
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<object> Rows { get; set; } = Array.Empty<object>().ToList();
    }
}
