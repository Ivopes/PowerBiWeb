namespace MetricsAPI.Models
{
    public class MetricPortion
    {
        public string Name { get; set; } = string.Empty;
        public string AdditionWithSignType { get; set; }
        public string AdditionWithSignName { get; set; } = string.Empty;
        public string AdditionWithoutSignName { get; set; } = string.Empty;
        public string AdditionWithoutSignType { get; set; }
        public List<MetricPortionRow> Rows { get; set; } = Enumerable.Empty<MetricPortionRow>().ToList();
        public string Description { get; set; } = string.Empty;
    }
}
