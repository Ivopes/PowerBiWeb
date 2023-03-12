using PowerBiWeb.Shared.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Project
{
    public class ReportDTO : DashboardDTO
    {
        public DatasetDTO? Dataset { get; set; }
    }
}
