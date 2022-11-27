using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using PowerBiWeb.Shared;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PowerBiWeb.Client.Utilities.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _anonymousState = new AuthenticationState(new ClaimsPrincipal());
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<CustomAuthStateProvider> _logger;
        public CustomAuthStateProvider(ILocalStorageService localStorage, ILogger<CustomAuthStateProvider> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            /*if (!await _localStorage.ContainKeyAsync("logged"))
            {
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                return _anonymousState;
            }*/
            //var s = await _localStorage.GetItemAsync<string>("logged");
            var user = await _localStorage.GetItemAsync<User>("logged");
            //var user = JsonSerializer.Deserialize<User>(s);
            if (user is null)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                return _anonymousState;
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }, "Fake authentication type");

            var userPrincipal = new ClaimsPrincipal(identity);

            var state = new AuthenticationState(userPrincipal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
       
    }
}
