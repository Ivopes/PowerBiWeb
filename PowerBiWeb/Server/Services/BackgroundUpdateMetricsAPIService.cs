using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Utilities.ConfigOptions;

namespace PowerBiWeb.Server.Services
{
    public class BackgroundUpdateMetricsAPIService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
        private readonly ILogger<BackgroundUpdateMetricsAPIService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DatasetsUpdateOptions _updateOptions;
        public BackgroundUpdateMetricsAPIService(ILogger<BackgroundUpdateMetricsAPIService> logger, IServiceProvider serviceProvider, IOptions<DatasetsUpdateOptions> opt)
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

            _logger.LogInformation($"Starting executing {nameof(BackgroundUpdateMetricsAPIService)}...");

            var diffToZeroMinutes = (48 - DateTime.UtcNow.Minute) % 60;
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
            await using var scope = _serviceProvider.CreateAsyncScope();

            var metricApiRepository = scope.ServiceProvider.GetRequiredService<IMetricsApiLoaderRepository>();

            var dbContext = scope.ServiceProvider.GetRequiredService<PowerBiContext>();

            var datasets = await dbContext.Datasets.ToListAsync();
            foreach (var dataset in datasets)
            {
                // Projekt byl zalozen a nejnovejsi data se stahli pri zalozeni
                if (dataset.LastUpdate.DayOfWeek == DayOfWeek.Saturday && DateTime.UtcNow.Subtract(dataset.LastUpdate).TotalHours < 24) continue;

                try
                {

                    var result = await metricApiRepository.GetMetricLatestAll(dataset.MetricFilesId);

                    if (result is not null && result.Count > 0)
                    {
                        // TODO: Smazat komentare pro produkci
                        //var metricSaver = scope.ServiceProvider.GetRequiredService<IMetricsSaverRepository>();
                        //await metricSaver.UploadMetric(result);
                        //project.LastUpdate = DateTime.UtcNow;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not auto-update metrics for dataset: {0}, with id: {1}", dataset.MetricFilesId, dataset.Id);
                }
            }

            //await dbContext.SaveChangesAsync();
        }
    }
}
