﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiWeb.Shared.Project
{
    public class ProjectDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string MetricUrl { get; set; } = string.Empty;
        public ICollection<UserProject> Users { get; set; } = Array.Empty<UserProject>();
    }
}