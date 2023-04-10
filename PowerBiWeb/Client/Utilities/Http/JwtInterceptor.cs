using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PowerBiWeb.Client.Utilities.Auth;

namespace PowerBiWeb.Client.Utilities.Http
{
    public class JwtInterceptor : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;

        public JwtInterceptor(ILocalStorageService localStorage, NavigationManager navigation)
        {
            _localStorage = localStorage;
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _localStorage.GetItemAsStringAsync(JwtAuthStateProvider.TokenKey);

            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);
            
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/login");
                //return null;
                var content = new StringContent("You are not authenticated. Try re-login");

                response.Content = content;
            }

            return response;
        }
    }
}
