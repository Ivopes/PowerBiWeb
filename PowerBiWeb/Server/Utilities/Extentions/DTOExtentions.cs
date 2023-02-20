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

            dto.Reports = new List<EmbedContentDTO>();

            foreach (var report in p.ProjectReports)
            {
                dto.Reports.Add(new()
                {
                    Name = report.Name,
                    Id = report.PowerBiId
                });
            }

            dto.Dashboards = new List<EmbedContentDTO>();

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
                Name = p.Name,
                DownloadContent = p.DownloadContent,
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
    }
}
