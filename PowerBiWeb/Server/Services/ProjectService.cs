using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api;
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
        private readonly IAppUserRepository _appUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMetricsSaverRepository _metricsSaverRepository;
        private readonly IMetricsApiLoaderRepository _metricsApiLoaderRepository;

        public ProjectService(IProjectRepository projectRepository, IHttpContextAccessor httpContext, IAppUserRepository appUserRepository, IMetricsSaverRepository metricsSaverRepository, IMetricsApiLoaderRepository metricsApiLoaderRepository)
        {
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContext;
            _appUserRepository = appUserRepository;
            _metricsSaverRepository = metricsSaverRepository;
            _metricsApiLoaderRepository = metricsApiLoaderRepository;
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

            //Update metrics
            var metrics = await _metricsApiLoaderRepository.GetMetricAllAsync(created.MetricFilesName, true);
            if (metrics is not null && metrics.Count > 0)
            {
                await _metricsSaverRepository.UploadMetric(created, metrics);
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
        public async Task<bool> IsMinEditor(int projectId)
        {
            var userId = GetUserId();

            var project = await _projectRepository.GetAsync(projectId);
            if (project is null) return false;
            if (!project.AppUserProject.Any(aup => aup.AppUserId == userId)) return false;

            var join = project.AppUserProject.Single(aup => aup.AppUserId == userId);

            return join.Role <= ProjectRoles.Editor;
        }
        public async Task<string> RemoveUserAsync(int userId, int projectId)
        {
            return await _projectRepository.RemoveUserAsync(userId, projectId);
        }
        
        public async Task<ProjectRoles?> GetProjectRole(int projectId)
        {
            var userId = GetUserId();

            var project = await _projectRepository.GetAsync(projectId);

            if (project is null) return null;
            if (!project.AppUserProject.Any(aup => aup.AppUserId == userId)) return null;

            var join = project.AppUserProject.Single(aup => aup.AppUserId == userId);

            return join.Role;
        }

        public async Task<string> RemoveProject(int projectId)
        {
            return await _projectRepository.RemoveProject(projectId);
        }

        #region Private Methods
        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        #endregion
    }
}
