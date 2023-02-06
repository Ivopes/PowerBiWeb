using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;

namespace PowerBiWeb.Server.Services
{
    public class BackgroundUpdateMetricsAPIService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1)); // upravit na 1 hour
        private readonly ILogger<BackgroundUpdateMetricsAPIService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundUpdateMetricsAPIService(ILogger<BackgroundUpdateMetricsAPIService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting executing {nameof(BackgroundUpdateMetricsAPIService)}...");

            //await UpdateMetricsForProjectsAsync();

            while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested)
            {
                var day = DateTime.UtcNow.DayOfWeek;
                var hour = DateTime.UtcNow.Hour;
                if (day == DayOfWeek.Sunday && hour == 3) // Update every sunday at 3 oclock (lowest usage)
                {
                    await UpdateMetricsForProjectsAsync();
                }
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
