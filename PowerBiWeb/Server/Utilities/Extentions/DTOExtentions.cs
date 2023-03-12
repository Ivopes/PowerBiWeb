using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Datasets;
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

            dto.Reports = new List<ReportDTO>();

            foreach (var report in p.ProjectReports)
            {
                dto.Reports.Add(new()
                {
                    Name = report.Name,
                    Id = report.PowerBiId,
                    Dataset = report.Dataset?.ToDTO()
                });
            }

            dto.Dashboards = new List<DashboardDTO>();

            foreach (var dashboard in p.ProjectDashboards)
            {
                dto.Dashboards.Add(new()
                {
                    Name = dashboard.Name,
                    Id = dashboard.PowerBiId
                });
            }

            return dto;
        }
        public static Project ToBO(this ProjectDTO p)
        {
            var created = new Project
            {
                Id = p.Id,
                Name = p.Name
            };

            return created;
        }
        public static DatasetDTO ToDTO(this PBIDataset pBIDataset)
        {
            var dataset = new DatasetDTO()
            {
                Id = pBIDataset.Id,
                PowerBiId = pBIDataset.PowerBiId,
                Name = pBIDataset.Name,
                MetricFilesId = pBIDataset.MetricFilesId,
                ColumnNames = pBIDataset.ColumnNames.ToArray(),
                ColumnTypes = pBIDataset.ColumnTypes.ToArray(),
                Measures = pBIDataset.Measures.ToArray(),
                MeasureDefinitions = pBIDataset.MeasureDefinitions.ToArray()
            };

            return dataset;
        }
        public static ProjectReport ToBO(this ReportDTO report)
        {
            var r = new ProjectReport
            {
                PowerBiId = report.Id,
                Name = report.Name,
                DatasetId = report.Dataset?.Id,
                PowerBIName = report.PowerBiName
            };

            r.Projects = new List<Project>();

            if (report.Projects.Count > 0)
            {
                r.Projects.Add(report.Projects[0].ToBO());
            }

            return r;
        }
        public static ProjectDashboard ToBO(this DashboardDTO dashboard)
        {
            var d = new ProjectDashboard
            {
                PowerBiId = dashboard.Id,
                Name = dashboard.Name,
                PowerBiName = dashboard.PowerBiName
            };

            d.Projects = new List<Project>();

            if (dashboard.Projects.Count > 0)
            {
                d.Projects.Add(dashboard.Projects[0].ToBO());
            }

            return d;
        }
    }
}
