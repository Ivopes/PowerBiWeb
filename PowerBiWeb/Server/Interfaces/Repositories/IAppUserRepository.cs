using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<string> ChangePasswordAsync(int userId, byte[] controlHash);
        Task<string> ChangeUsernameAsync(int userId, string newUsername);
        Task<IEnumerable<ApplUser>> GetAllAsync();
        Task<ApplUser?> GetByEmailAsync(string email);
        Task<ApplUser?> GetByIdAsync(int id);
        Task<string> PostAsync(ApplUser user);
    }
}
