using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IAuthService
    {
        string Login(User user);
    }
}