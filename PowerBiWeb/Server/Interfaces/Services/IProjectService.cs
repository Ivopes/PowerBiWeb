using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IProjectService
    {
        Task<string> AddToUser(AddUserToObjectDTO dto);
        Task<List<ProjectDTO>> GetAllAsync();
        Task<ProjectDTO?> GetAsync(int projectId);
        /// <summary>
        /// Create new project and add it to the creator
        /// </summary>
        Task<ProjectDTO> PostAsync(ProjectDTO project);
        /// <summary>
        /// Does user have atleast Editor privilege
        /// </summary>
        Task<bool> IsMinEditor(int projectId);
    }
}