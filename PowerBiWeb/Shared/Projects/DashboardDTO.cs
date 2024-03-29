﻿namespace PowerBiWeb.Shared.Projects
{
    public class DashboardDTO
    {
        // Id of Power BI report to be embedded
        public Guid Id { get; set; }

        // Name of the content
        public string Name { get; set; } = string.Empty;
        // Name of the content
        public string PowerBiName { get; set; } = string.Empty;
        // Embed URL for the Power BI report
        public string EmbedUrl { get; set; } = string.Empty;
        // Embed Token for the Power BI report
        public string EmbedToken { get; set; } = string.Empty;
        
        public List<ProjectDTO> Projects { get; set; } = Array.Empty<ProjectDTO>().ToList();
    }
}
