using PowerBiWeb.Shared.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Project
{
    public class ReportDTO : EmbedContentDTO
    {
        public DatasetDTO? Dataset { get; set; }
        public List<ProjectDTO> Projects { get; set; } = Array.Empty<ProjectDTO>().ToList();
    }
}
