using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Shared.Projects
{
    public class ReportDTO : DashboardDTO
    {
        public DatasetDTO? Dataset { get; set; }
    }
}
