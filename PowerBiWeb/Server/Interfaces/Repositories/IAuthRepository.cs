using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        string Login(User user);
    }
}