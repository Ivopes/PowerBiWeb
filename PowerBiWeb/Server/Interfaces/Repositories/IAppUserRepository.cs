using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<ApplUser>> GetAsync();
        Task<string> PostAsync(ApplUser user);
    }
}
