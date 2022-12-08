using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PowerBiWeb.Client;
using PowerBiWeb.Client.Utilities.Auth;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap5;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage(opt =>
{
    //opt.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddBlazoredToast();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

await builder.Build().RunAsync();
