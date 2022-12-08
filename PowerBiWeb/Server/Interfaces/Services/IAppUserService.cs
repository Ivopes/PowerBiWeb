using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUser>> GetAsync();
    }
}
