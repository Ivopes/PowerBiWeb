using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Utilities.Extentions
{
    public static class DTOExtentions
    {
        public static ProjectDTO ToDTO(this Project p)
        {
            var dto = new ProjectDTO
            {
                Id = p.Id,
                Name = p.Name
            };

            dto.Users = new List<UserDTO>();

            foreach (var user in p.AppUserProjects)
            {
                dto.Users.Add(new()
                {
                    Id = user.AppUserId,
                    Email = user.AppUser.Email,
                    Username = user.AppUser.Username,
                    Role = (ProjectRoleDTO)user.Role,
                });
            }

            dto.Reports = new List<EmbedReportDTO>();

            foreach (var report in p.ProjectReports)
            {
                dto.Reports.Add(new()
                {
                    ReportName = report.Name,
                    ReportId = report.PowerBiId
                });
            }

            dto.Dashboards = new List<EmbedReportDTO>();

            foreach (var dashboard in p.ProjectDashboards)
            {
                dto.Dashboards.Add(new()
                {
                    ReportName = dashboard.Name,
                    ReportId = dashboard.PowerBiId
                });
            }

            dto.ConnectedMetricsIds = string.Join(';', p.Datasets.Select(d => d.MetricFilesId));

            dto.PowerBiPrefix = p.PowerBiPrefix;

            return dto;
        }
        public static Project ToBO(this ProjectDTO p)
        {
            var created = new Project
            {
                Id = p.Id,
                Name = p.Name,
                PowerBiPrefix = p.PowerBiPrefix,
                DownloadContent = p.DownloadContent,
                
            };

            created.Datasets = new List<PBIDataset>();

            var ids = p.ConnectedMetricsIds.Split(';');

            foreach (var id in ids)
            {
                created.Datasets.Add(new()
                {
                    MetricFilesId = id,
                });
            }

            return created;
        }
    }
}
