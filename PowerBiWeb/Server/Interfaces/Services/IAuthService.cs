using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(User user);
    }
}