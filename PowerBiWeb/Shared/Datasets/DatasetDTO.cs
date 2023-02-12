using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Datasets
{
    public class DatasetDTO
    {
        public int Id { get; set; }
        public string MetricFilesId { get; set; } = string.Empty;
        public Guid PowerBiId { get; set; }
        public string[] ColumnNames { get; set; } = Array.Empty<string>();
        public string[] ColumnTypes { get; set; } = Array.Empty<string>();
        public string[] Measures { get; set; } = Array.Empty<string>(); 
        public string[] MeasureDefinitions { get; set; } = Array.Empty<string>();
    }
}
