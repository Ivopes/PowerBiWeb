namespace MetricsAPI.Models
{
    public class MetricPortionRow
    {
        public DateTime Date { get; set; }
        public float AdditionWithSign { get; set; }
        public float AdditionWithoutSign { get; set; }
        public string Release { get; set; } = string.Empty;

    }
}
