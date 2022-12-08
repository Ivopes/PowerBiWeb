using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<IEnumerable<ApplUser>> GetAsync();
        Task<string> PostAsync(UserRegisterInformation user);
    }
}
