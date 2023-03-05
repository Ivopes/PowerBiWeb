using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared.Project;
using System.Security.Claims;

namespace PowerBiWeb.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDatasetRepository _datasetRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMetricsContentRepository _metricsContentRepository;
        private readonly IMetricsApiLoaderRepository _metricsApiLoaderRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IProjectRepository projectRepository, IHttpContextAccessor httpContext, IAppUserRepository appUserRepository, IMetricsContentRepository metricsSaverRepository, IMetricsApiLoaderRepository metricsApiLoaderRepository, IReportRepository reportRepository, IDashboardRepository dashboardRepository, IDatasetRepository datasetRepository, ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContext;
            _appUserRepository = appUserRepository;
            _metricsContentRepository = metricsSaverRepository;
            _metricsApiLoaderRepository = metricsApiLoaderRepository;
            _reportRepository = reportRepository;
            _dashboardRepository = dashboardRepository;
            _datasetRepository = datasetRepository;
            _logger = logger;
        }

        public async Task<List<ProjectDTO>> GetAllAsync()
        {
            int userId = GetUserId();

            var p = await _projectRepository.GetAllAsync(userId);

            var result = new List<ProjectDTO>();

            foreach (var project in p)
            {
                var newP = project.ToDTO();
                result.Add(newP);
            }

            return result;
        }

        public async Task<ProjectDTO?> GetAsync(int projectId)
        {
            var p = await _projectRepository.GetAsync(projectId);

            if (p is null) return null;

            var newP = p.ToDTO();

            return newP;
        }
        public async Task<ProjectDTO> PostAsync(ProjectDTO project)
        {
            var p = project.ToBO();

            int userId = GetUserId();

            var created = await _projectRepository.Post(userId, p);

            project.Id = created.Id;

            if (created.DownloadContent)
            {
                //Download content from Power BI
                await _reportRepository.UpdateReportsAsync(created.Id);
                await _dashboardRepository.UpdateDashboardsAsync(created.Id);
            }

            return project;
        }
        public async Task<string> AddToUserAsync(UserToProjectDTO dto)
        {
            if (!Enum.IsDefined(typeof(ProjectRoles), (int)dto.Role)) return "Role value does not exist";

            ProjectRoles role = (ProjectRoles)dto.Role;

            return await _projectRepository.AddToUserAsync(dto.UserEmail, dto.ProjectId, role);
        }
        public async Task<string> EditUserAsync(UserToProjectDTO dto)
        {
            if (!Enum.IsDefined(typeof(ProjectRoles), (int)dto.Role)) return "Role value does not exist";

            ProjectRoles role = (ProjectRoles)dto.Role;

            return await _projectRepository.EditUserAsync(dto.UserEmail, dto.ProjectId, role);
        }
        public async Task<string> EditProject(int projectId, ProjectDTO dto)
        {
            var p = dto.ToBO();

            return await _projectRepository.EditProject(projectId, p);
        }
        public async Task<string> RemoveUserAsync(int userId, int projectId)
        {
            var project = await _projectRepository.GetAsync(projectId);

            if (project is null) return "Project not found";

            if (project.AppUserProjects.Count == 1) return "Cant leave project when alone. Delete it instead";

            return await _projectRepository.RemoveUserAsync(userId, projectId);
        }
        public async Task<ProjectRoles?> GetProjectRole(int projectId)
        {
            var userId = GetUserId();

            var project = await _projectRepository.GetAsync(projectId);

            if (project is null) return null;
            if (!project.AppUserProjects.Any(aup => aup.AppUserId == userId)) return null;

            var join = project.AppUserProjects.Single(aup => aup.AppUserId == userId);

            return join.Role;
        }

        public async Task<string> RemoveProject(int projectId)
        {
            return await _projectRepository.RemoveProject(projectId);
        }
        public async Task<string> AddReportAsync(int projectId, EmbedContentDTO report)
        {
            ProjectReport r = new()
            {
                Name = report.Name,
                PowerBiId = report.Id
            };

            return await _metricsContentRepository.AddReportsAsync(projectId, r);
        }
        public async Task<string> RemoveReportAsync(int projectId, Guid reportId)
        {
            return await _projectRepository.RemoveReportsAsync(projectId, reportId);
        }
        public async Task<string> AddDashboardAsync(int projectId, EmbedContentDTO dashboard)
        {
            ProjectDashboard d = new()
            {
                Name = dashboard.Name,
                PowerBiId = dashboard.Id
            };

            return await _metricsContentRepository.AddDashboardsAsync(projectId, d);
        }
        public async Task<string> RemoveDashboardAsync(int projectId, Guid dashboardId)
        {
            return await _projectRepository.RemoveDashboardsAsync(projectId, dashboardId);
        }
        #region Private Methods
        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        #endregion
    }
}
