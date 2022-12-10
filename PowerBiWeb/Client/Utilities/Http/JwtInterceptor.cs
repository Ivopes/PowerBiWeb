using Blazored.LocalStorage;
using PowerBiWeb.Client.Utilities.Auth;

namespace PowerBiWeb.Client.Utilities.Http
{
    public class JwtInterceptor : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public JwtInterceptor(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _localStorage.GetItemAsStringAsync(JwtAuthStateProvider.TokenKey);

            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
