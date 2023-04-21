using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PowerBiWeb.Server.Interfaces.Repositories;
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

            _logger.LogInformation($"Starting executing {nameof(BackgroundUpdateMetricsApiService)}...");
            
            var diffToZeroMinutes = (60 - DateTime.UtcNow.Minute) % 60;
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

            await using var scope = _serviceProvider.CreateAsyncScope();

            var metricApiRepository = scope.ServiceProvider.GetRequiredService<IMetricsApiLoaderRepository>();

            var dbContext = scope.ServiceProvider.GetRequiredService<PowerBiContext>();

            var datasets = await dbContext.Datasets.ToListAsync();
            foreach (var dataset in datasets)
            {
                try
                {
                    var result = await metricApiRepository.GetMetricIncrement(dataset.MetricFilesId);

                    if (result is not null)
                    {
                        // TODO: Smazat komentare pro produkci
                        var metricSaver = scope.ServiceProvider.GetRequiredService<IMetricsContentRepository>();
                        await metricSaver.AddRowsToDataset(dataset, result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not auto-update metrics for dataset: {0}, with id: {1}", dataset.MetricFilesId, dataset.Id);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
