using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplUser?> LoginAsync(UserLoginInformation user);
    }
}