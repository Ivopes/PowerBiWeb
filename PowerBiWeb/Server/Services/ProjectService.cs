using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared;
using System.Security.Claims;

namespace PowerBiWeb.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectService(IProjectRepository projectRepository, IHttpContextAccessor httpContext, IAppUserRepository appUserRepository)
        {
            _projectRepository = projectRepository;
            _httpContextAccessor = httpContext;
            _appUserRepository = appUserRepository;
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

        public async Task<ProjectDTO?> GetAsync(int id)
        {
            var p = await _projectRepository.GetAsync(id);

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

            return project;
        }
        public async Task<string> AddToUser(AddUserToObjectDTO dto)
        {
            if (!Enum.IsDefined(typeof(ProjectRoles), dto.Role)) return "Role value does not exist";

            ProjectRoles role = (ProjectRoles)dto.Role;

            return await _projectRepository.AddToUser(dto.UserEmail, dto.ProjectId, role);
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
        private int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
