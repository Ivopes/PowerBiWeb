using MetricsAPI.Models;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Server.Utilities.PowerBI;
using System.Text.Json;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using System.Text;
using System.Net.Http.Headers;

namespace PowerBiWeb.Server.Repositories
{
    public class PowerBiRepository : IMetricsSaverRepository
    {
        private readonly AadService _aadService;
        private readonly Guid _workspaceId;
        private readonly ILogger _logger;
        public PowerBiRepository(AadService aadService, ILogger<PowerBiRepository> logger)
        {
            _aadService = aadService;
            _workspaceId = Guid.Parse(_aadService.WorkspaceId);
            _logger = logger;
        }

        public async Task UploadMetric(MetricPortion metric)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var datasets = await pbiClient.Datasets.GetDatasetsInGroupAsync(_workspaceId);

            string datasetId = string.Empty;
            foreach (var dataset in datasets.Value)
            {
                if (dataset.Name == metric.Name)
                {
                    datasetId = dataset.Id;

                    break;
                }
            }
            
            if (string.IsNullOrEmpty(datasetId)) // Create new dataset
            {
                var dt = await CreateMetricDataset(metric);
                if (dt is null)
                {
                    return;
                }
                datasetId = dt.Id;
            }

            //Add rows
            foreach (var row in metric.Rows)
            {
                row.Release += 2;
            }

            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                    {
                        new PowerBiRowJsonConverter()
                    }
            };

            var json = JsonSerializer.Serialize(metric, serializeOptions);

            try
            { 
                // Must use HttpClient because power bi SDK cant handle custom serialization
                var httpClient = new HttpClient();
                var url = $"https://api.powerbi.com/v1.0/myorg/groups/{_workspaceId}/datasets/{datasetId}/tables/{metric.Name}/rows";

                httpClient.BaseAddress = new Uri(url);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aadService.GetAccessToken());

                var response = await httpClient.PostAsync(string.Empty, content);
                if (!response.IsSuccessStatusCode)
                {
                    string r = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Could not post rows: {r}");
                }
            }
            catch (Microsoft.Rest.HttpOperationException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
            //return stringData;
        }
        public async Task UploadMetric(List<MetricPortion> metrics)
        {
            foreach (var metric in metrics)
            {
                await UploadMetric(metric);
            }
        }
        private async Task<Dataset?> CreateMetricDataset(MetricPortion metric)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var tables = new List<Table>();
            var table = new Table()
            {
                Name = metric.Name,
                Description = metric.Description
            };

            var columns = new List<Column> //Int64, Double, Boolean, Datetime, String
            {
                new Column("Datum", "Datetime"),
                new Column(metric.AdditionWithSignName, TypeToPowerBiType(metric.AdditionWithSignType)),
                new Column(metric.AdditionWithoutSignName, TypeToPowerBiType(metric.AdditionWithoutSignType)),
                new Column("Release", "String"),
            };

            var measures = new List<Measure>
            {
                new Measure("SumCelkem", $"CALCULATE (SUM({metric.Name}[{metric.AdditionWithoutSignName}]), FILTER( ALL( {metric.Name} ), {metric.Name}[Datum] <= MAX ( {metric.Name}[Datum] )))"),
                new Measure("SumCelkemByRelease", $"CALCULATE (SUM ({metric.Name}[{metric.AdditionWithoutSignName}] ), FILTER(ALL ( {metric.Name} ), {metric.Name}[Datum] <= MAX ( {metric.Name}[Datum] )), VALUES({metric.Name}[Release]))"),
                                
                new Measure("SumPriznak", $"CALCULATE ( SUM({metric.Name}[{metric.AdditionWithSignName}] ), FILTER ( ALL ( {metric.Name} ), {metric.Name}[Datum] <= MAX ( {metric.Name}[Datum] )))"),
                new Measure("SumPriznakByRelease", $"CALCULATE ( SUM ( {metric.Name}[{metric.AdditionWithSignName}] ), FILTER ( ALL ( {metric.Name} ), {metric.Name}[Datum] <= MAX ( {metric.Name}[Datum])), VALUES({metric.Name}[Release]))"),

                new Measure("Podil", $"{metric.Name}[SumPriznak] / {metric.Name}[SumCelkem]"),
                new Measure("PodilPodleRelease", $"{metric.Name}[SumPriznakByRelease] / {metric.Name}[SumCelkemByRelease]"),

            };
            
            table.Columns = columns;
            table.Measures = measures;

            tables.Add(table);

            var pushDatasetRequest = new CreateDatasetRequest(metric.Name, tables);

            try
            {
                Dataset datasetResult = await pbiClient.Datasets.PostDatasetInGroupAsync(_workspaceId, pushDatasetRequest);
                return datasetResult;

            }
            catch (Microsoft.Rest.HttpOperationException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
            return null;
        }
        private string TypeToPowerBiType(string type)
        {
            return type switch //Int64, Double, Boolean, Datetime, String
            {
                "System.Int32" => "Int64",
                "System.Single" => "Double",
                "System.Double" => "Double",
                "System.Boolean" => "Boolean",
                "System.DateTime" => "Datetime",
                "System.String" => "String",
            }; 
        }
    }
}
