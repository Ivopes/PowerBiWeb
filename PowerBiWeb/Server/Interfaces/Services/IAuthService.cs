using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ProjectRoles?> GetProjectRole(int projectId);
        Task<string> LoginAsync(UserLoginInformation user);
    }
}