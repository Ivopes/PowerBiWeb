namespace PowerBiWeb.Server.Models.Metrics
{
    public class MetricPortion
    {
        public string Name { get; set; } = string.Empty;
        public string AdditionWithSignType { get; set; } = string.Empty;
        public string AdditionWithSignName { get; set; } = string.Empty;
        public string AdditionWithoutSignName { get; set; } = string.Empty;
        public string AdditionWithoutSignType { get; set; } = string.Empty;
        public List<MetricPortionRow> Rows { get; set; } = Enumerable.Empty<MetricPortionRow>().ToList();
        public string Description { get; set; } = string.Empty;
    }
}
