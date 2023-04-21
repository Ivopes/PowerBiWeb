using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Utilities.ConfigOptions;

namespace PowerBiWeb.Server.Services;

public class BackgroundDownloadMetricsService : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly ILogger<BackgroundDownloadMetricsService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly DownloadPbixOptions _updateOptions;

    public BackgroundDownloadMetricsService(ILogger<BackgroundDownloadMetricsService> logger, IServiceProvider serviceProvider, IOptions<DownloadPbixOptions> opt)
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
                Enabled = opt.Value.Enabled,
                SavePath = opt.Value.SavePath
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

        _logger.LogInformation($"Starting executing {nameof(BackgroundDownloadMetricsService)}...");

        string path;
        if (Path.IsPathFullyQualified(_updateOptions.SavePath))
        {
            path = _updateOptions.SavePath;
        }
        else
        {
            path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _updateOptions.SavePath);
        }
        
        var diffToZeroMinutes = (60 - DateTime.UtcNow.Minute) % 60;
        await Task.Delay(TimeSpan.FromMinutes(diffToZeroMinutes), stoppingToken);

        if (ShouldUpdate()) 
            await DownloadPbixAsync(path);

        while (await _timer.WaitForNextTickAsync(stoppingToken)
               && !stoppingToken.IsCancellationRequested)
        {
            if (ShouldUpdate())
            {
                await DownloadPbixAsync(path);
            }
        }
    }

    private async Task DownloadPbixAsync(string path)
    {
        _logger.LogInformation($"Starting {nameof(BackgroundDownloadMetricsService)} update...");

        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PowerBiContext>();

        var powerBiRepository = scope.ServiceProvider.GetRequiredService<IMetricsContentRepository>();
        
        var reports = await dbContext.ProjectReports.ToListAsync();
        path = Path.Combine(path, DateTime.UtcNow.ToString("dd_MM_yyyy_HH"));
        Directory.CreateDirectory(path);
        foreach (var report in reports)
        {
            try
            {
                var stream = (await powerBiRepository.GetDownloadedReportAsync(report.PowerBiId))!;

                var filePath = Path.Combine(path, report.Name + ".pbix");
                
                await using var filestream = new FileStream(filePath, FileMode.Create);

                await stream.CopyToAsync(filestream);
                
                filestream.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not download pbix file for report: {0}", report.PowerBiId);
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
}