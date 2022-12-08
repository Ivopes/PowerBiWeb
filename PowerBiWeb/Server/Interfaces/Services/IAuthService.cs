using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserLoginInformation user);
    }
}