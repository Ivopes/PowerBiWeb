namespace PowerBiWeb.Server.Models.Metrics
{
    public class MetricData
    {
        public ICollection<object> Rows { get; set; } = Array.Empty<object>().ToList();
    }
}
