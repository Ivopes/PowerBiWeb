using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using PowerBiWeb.Shared;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace PowerBiWeb.Client.Utilities.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthType = "JwtAuth";
        private readonly AuthenticationState _anonymousState = new AuthenticationState(new ClaimsPrincipal());
        private readonly ILocalStorageService _localStorage;
        private readonly ILogger<JwtAuthStateProvider> _logger;
        public JwtAuthStateProvider(ILocalStorageService localStorage, ILogger<JwtAuthStateProvider> logger)
        {
            _localStorage = localStorage;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var token = await _localStorage.GetItemAsync<string>("accessToken");
            
            if (token is null)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                return _anonymousState;
            }

            var securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var identity = new ClaimsIdentity(new List<Claim>(securityToken.Claims), AuthType);

            var userPrincipal = new ClaimsPrincipal(identity);

            var state = new AuthenticationState(userPrincipal);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
       
    }
}
