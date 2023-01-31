using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IProjectService
    {
        Task<string> AddToUserAsync(UserToProjectDTO dto);
        Task<List<ProjectDTO>> GetAllAsync();
        Task<ProjectDTO?> GetAsync(int projectId);
        /// <summary>
        /// Create new project and add it to the creator
        /// </summary>
        Task<ProjectDTO> PostAsync(ProjectDTO project);
        Task<ProjectRoles?> GetProjectRole(int projectId);
        Task<string> EditUserAsync(UserToProjectDTO dto);
        Task<string> RemoveUserAsync(int userId, int projectId);
        Task<string> RemoveProject(int projectId);
    }
}