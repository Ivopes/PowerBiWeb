using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Utilities.ConfigOptions;

namespace PowerBiWeb.Server.Services;

public class BackgroundUpdateContentService : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly ILogger<BackgroundUpdateContentService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodUpdateOptions _updateOptions;
    
    public BackgroundUpdateContentService(ILogger<BackgroundUpdateContentService> logger, IServiceProvider serviceProvider, IOptions<ContentUpdateOptions> opt)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        if (opt.Value is not null)
        {
            _updateOptions = new()
            {
                DayOfWeek = opt.Value.DayOfWeek,
                Hour = opt.Value.Hour,
                UpdateFrequency = opt.Value.UpdateFrequency,
                Enabled = opt.Value.Enabled
            };
        }
        else
        {
            _updateOptions = new()
            {
                Enabled = false
            };
        }

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_updateOptions.Enabled) return;

        _logger.LogInformation($"Starting executing {nameof(BackgroundUpdateContentService)}...");
        return;
        var diffToZeroMinutes = (60 - DateTime.UtcNow.Minute) % 60;
        await Task.Delay(TimeSpan.FromMinutes(diffToZeroMinutes), stoppingToken);

        if (ShouldUpdate()) 
            await UpdateContentForProjectsAsync();

        while (await _timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
            if (ShouldUpdate())
            {
                await UpdateContentForProjectsAsync();
            }
        }
    }
    private bool ShouldUpdate()
    {
        var now = DateTime.UtcNow;
        switch (_updateOptions.UpdateFrequency)
        {
            case UpdateFrequency.Hour:
                return true;
            case UpdateFrequency.Day:
                return now.Hour == _updateOptions.Hour;
            case UpdateFrequency.Week:
                return now.Hour == _updateOptions.Hour && now.DayOfWeek == _updateOptions.DayOfWeek;
            default:
                return false;
        }
    }
    private async Task UpdateContentForProjectsAsync()
    {
        _logger.LogInformation($"Starting {nameof(BackgroundUpdateContentService)} update...");

        await using var scope = _serviceProvider.CreateAsyncScope();

        var powerBiRepository = scope.ServiceProvider.GetRequiredService<IMetricsContentRepository>();

        var dbContext = scope.ServiceProvider.GetRequiredService<PowerBiContext>();

        var projects = await dbContext.Projects.ToListAsync();
        foreach (var project in projects)
        {
            try
            {
                await powerBiRepository.UpdateReportsAsync(project.Id);
                await powerBiRepository.UpdateDashboardsAsync(project.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not auto-update content for project {0}", project.Id);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}