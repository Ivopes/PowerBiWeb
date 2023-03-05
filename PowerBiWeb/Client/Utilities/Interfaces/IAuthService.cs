using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IAuthService
    {
        Task<HttpResponse<string>> LoginAsync(string username, string password);
        Task<HttpResponse<string>> LoginAsync(UserLoginInformation loginRequest);
        Task<HttpResponse> RegisterAsync(UserRegisterInformation registerRequest);
    }
}
