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

            return dto;
        }
        public static Project ToBO(this ProjectDTO p)
        {
            return new Project
            {
                Id = p.Id,
                Name = p.Name,
                MetricFilesName = p.MetricName
            };
        }
    }
}
