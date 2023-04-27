using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Utilities.ConfigOptions;

namespace PowerBiWeb.Server.Services
{
    public class BackgroundUpdateMetricsApiService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
        private readonly ILogger<BackgroundUpdateMetricsApiService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly PeriodUpdateOptions _updateOptions;
        public BackgroundUpdateMetricsApiService(ILogger<BackgroundUpdateMetricsApiService> logger, IServiceProvider serviceProvider, IOptions<DatasetUpdateOptions> opt)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            if (opt.Value is not null)
            {
                _updateOptions = opt.Value;
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

            _logger.LogInformation($"Starting executing {nameof(BackgroundUpdateMetricsApiService)}...");

            var diffToZeroMinutes = (_updateOptions.Minute - DateTime.UtcNow.Minute + 60) % 60;
            await Task.Delay(TimeSpan.FromMinutes(diffToZeroMinutes), stoppingToken);

            if (ShouldUpdate()) 
                await UpdateMetricsForProjectsAsync();

            while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
            {
                if (ShouldUpdate())
                {
                    await UpdateMetricsForProjectsAsync();
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
        private async Task UpdateMetricsForProjectsAsync()
        {
            _logger.LogInformation($"Starting {nameof(BackgroundUpdateMetricsApiService)} update...");

            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var datasetService = scope.ServiceProvider.GetRequiredService<IDatasetService>();

                await datasetService.UpdateAllAsync();
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Could not auto-update metrics");
            }
        }
    }
}
