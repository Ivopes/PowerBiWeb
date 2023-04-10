namespace PowerBiWeb.Server.Utilities
{
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Client;
    using PowerBiWeb.Server.Utilities.ConfigOptions;
    using System;
    using System.Linq;
    using System.Security;

    public class AadService
    {
        private readonly IOptions<PowerBiOptions> azureAd;
        public string WorkspaceId => azureAd.Value.WorkspaceId;
        public string PowerBiUrl => azureAd.Value.PowerBiApiUrl;
        private AuthenticationResult? _authenticationResult;
        private readonly object _accessLock = new object();
        public AadService(IOptions<PowerBiOptions> azureAd)
        {
            this.azureAd = azureAd;
        }

        /// <summary>
        /// Generates and returns Access token
        /// </summary>
        /// <returns>AAD token</returns>
        public string GetAccessToken()
        {
            AuthenticationResult? authenticationResult = null;

            if (_authenticationResult is not null && DateTime.UtcNow < _authenticationResult.ExpiresOn)
            {
                return _authenticationResult.AccessToken;
            }
                
            lock (_accessLock)
            {
                // Druha kontrola, v pripade ze vice vlaken se chce prihlasit najednou. 
                if (_authenticationResult is not null && DateTime.UtcNow < _authenticationResult.ExpiresOn)
                {
                    return _authenticationResult.AccessToken;
                }
                IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(azureAd.Value.ClientId).WithAuthority(azureAd.Value.AuthorityUrl).Build();
                // Create a public client to authorize the app with the AAD app
                try
                {
                    /*
                     authenticationResult = clientApp.AcquireTokenByUsernamePassword(
                        azureAd.Value.ScopeBase,
                        azureAd.Value.PbiUsername,
                        azureAd.Value.PbiPassword)
                        .ExecuteAsync().Result;
                    */
                    var userAccounts = clientApp.GetAccountsAsync().Result;
                    // Retrieve Access token from cache if available
                    authenticationResult = clientApp.AcquireTokenSilent(azureAd.Value.ScopeBase, userAccounts.FirstOrDefault()).ExecuteAsync().Result;
                }
                catch (Exception ex)
                {
                    authenticationResult = clientApp.AcquireTokenByUsernamePassword(
                        azureAd.Value.ScopeBase, 
                        azureAd.Value.PbiUsername,
                        azureAd.Value.PbiPassword)
                        .ExecuteAsync().Result;
                }

                _authenticationResult = authenticationResult;
                return authenticationResult!.AccessToken;
            }
        }
    }
}
