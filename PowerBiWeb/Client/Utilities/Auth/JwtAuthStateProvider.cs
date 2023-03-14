using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PowerBiWeb.Client.Utilities.Auth
{
    public class JwtAuthStateProvider : TokenAuthStateProvider
    {
        private const string AuthType = "JwtAuth";
        public const string TokenKey = "AccessToken";
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
            var token = await _localStorage.GetItemAsync<string>(TokenKey);

            var handler = new JwtSecurityTokenHandler();
            
            if (token is null || !handler.CanReadToken(token))
            {
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                return _anonymousState;
            }

            JwtSecurityToken securityToken;
            try
            {
                securityToken = handler.ReadJwtToken(token);
                if (securityToken.ValidTo < DateTime.UtcNow)
                {
                    NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                    return _anonymousState;
                }  
            }
            catch (ArgumentException)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymousState));
                return _anonymousState;
            }

            var identity = new ClaimsIdentity(new List<Claim>(securityToken.Claims), AuthType);

            var userPrincipal = new ClaimsPrincipal(identity);

            var state = new AuthenticationState(userPrincipal);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public override async Task SaveToken(string token)
        {
            await _localStorage.SetItemAsStringAsync(TokenKey, token);
        }
        public override async Task RemoveToken()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
        public override async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsStringAsync(TokenKey);
        }
    }
    public abstract class TokenAuthStateProvider : AuthenticationStateProvider
    {
        public abstract Task SaveToken(string token);
        public abstract Task RemoveToken();
        public abstract Task<string> GetToken();
    }
}
