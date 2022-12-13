using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<IEnumerable<ApplUser>> GetAsync();
        Task<UserDetail?> GetByIdAsync(int id);
        Task<string> PostAsync(UserRegisterInformation user);
    }
}
