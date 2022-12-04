using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared
{
    public class Project
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string MetricUrl { get; set; } = string.Empty;
    }
}
