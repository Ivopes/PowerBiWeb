using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Repositories;
using PowerBiWeb.Server.Services;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Server.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EntityFramework.Exceptions.SqlServer;
using PowerBiWeb.Server.Utilities.ConfigOptions;
using Microsoft.Extensions.DependencyInjection;
using PowerBiWeb.Server.Utilities.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient(typeof(AadService));
builder.Services.Configure<PowerBiOptions>(builder.Configuration.GetSection("PowerBiOptions"));
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IAppUserService, AppUserService>();
builder.Services.AddTransient<IAppUserRepository, AppUserRepository>();
builder.Services.AddHttpClient(HttpClientTypes.MetricsApi, c => c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PowerBiMetricsUrl")!));
builder.Services.AddTransient<IMetricsApiLoaderRepository, MetricsApiRepository>();
builder.Services.AddTransient<IMetricsSaverRepository, PowerBiRepository>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddDbContext<PowerBiContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("powerBiDb"));
    options.UseExceptionProcessor();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecretKey"]))
    };
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
