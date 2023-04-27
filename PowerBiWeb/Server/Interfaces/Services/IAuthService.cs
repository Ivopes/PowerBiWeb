using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ProjectRoles?> GetProjectRole(int projectId);
        Task<ProjectRoles?> GetProjectRole(int projectId, int userId);
        Task<ProjectRoles?> GetProjectRole(int projectId, string userEmail);
        Task<string> LoginAsync(UserLoginInformation user);
    }
}