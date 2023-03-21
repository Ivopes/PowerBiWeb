using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<string> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<string> ChangeUsernameAsync(int userId, string newUsername);
        Task<IEnumerable<ApplUser>> GetAsync();
        Task<UserDetail?> GetByIdAsync(int id);
        Task<string> PostAsync(UserRegisterInformation user);
    }
}
