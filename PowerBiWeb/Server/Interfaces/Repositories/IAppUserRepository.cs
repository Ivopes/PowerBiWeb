using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<ApplUser>> GetAllAsync();
        Task<ApplUser?> GetByIdAsync(int id);
        Task<string> PostAsync(ApplUser user);
    }
}
