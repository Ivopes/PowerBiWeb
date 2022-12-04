using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<string> LoginAsync(User user);
    }
}