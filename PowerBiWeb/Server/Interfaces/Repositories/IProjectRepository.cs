using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync(int userId);
        Task<Project?> GetAsync(int projectId);
        /// <summary>
        /// Create new project and add it to the creator
        /// </summary>
        Task<Project> Post(int userId, Project project);
        /// <summary>
        /// Add project to user
        /// </summary>
        Task<string> AddToUserAsync(string userEmail, int projectId, ProjectRoles role);
        Task<string> EditUserAsync(string userEmail, int projectId, ProjectRoles newRole);
    }
}
