using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ProjectRoles?> GetProjectRole(int projectId);
        Task<string> LoginAsync(UserLoginInformation user);
    }
}