﻿using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Shared
{
    public class EmbedParams
    {
        // Type of the object to be embedded
        public string Type { get; set; } = string.Empty;

        // Report to be embedded
        public List<EmbedContentDTO> EmbedReport { get; set; } = Enumerable.Empty<EmbedContentDTO>().ToList();

        // Embed Token for the Power BI report
        public EmbedToken EmbedToken { get; set; } = new();
    }
}
