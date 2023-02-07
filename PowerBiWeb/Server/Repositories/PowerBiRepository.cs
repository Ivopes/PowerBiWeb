using MetricsAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Server.Utilities.PowerBI;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace PowerBiWeb.Server.Repositories
{
    public class PowerBiRepository : IMetricsSaverRepository
    {
        private readonly AadService _aadService;
        private readonly Guid _workspaceId;
        private readonly ILogger<PowerBiRepository> _logger;
        private readonly PowerBiContext _dbContext;
        public PowerBiRepository(AadService aadService, ILogger<PowerBiRepository> logger, PowerBiContext dbContext)
        {
            _aadService = aadService;
            _workspaceId = Guid.Parse(_aadService.WorkspaceId);
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<EmbedReportDTO> GetEmbededReportAsync(Guid reportId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var report = await pbiClient.Reports.GetReportInGroupAsync(_workspaceId, reportId);

            EmbedToken embedToken = await GetEmbedReportToken(reportId, new Guid(report.DatasetId), _workspaceId);

            return new()
            {
                ReportName = report.Name,
                ReportId = reportId,
                EmbedToken = embedToken.Token,
                EmbedUrl = report.EmbedUrl
            };
        }
        public async Task<EmbedReportDTO> GetEmbededDashboardAsync(Guid dashboardId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var dashboard = await pbiClient.Dashboards.GetDashboardInGroupAsync(_workspaceId, dashboardId);

            EmbedToken embedToken = await GetEmbedDashboardTokenAsync(dashboardId, _workspaceId);

            return new()
            {
                ReportName = dashboard.DisplayName,
                ReportId = dashboardId,
                EmbedToken = embedToken.Token,
                EmbedUrl = dashboard.EmbedUrl
            };
        }
        public async Task<string> UpdateReportsAsync(int projectId)
        {
            var projectEntity = await _dbContext.Projects.FindAsync(projectId);

            if (projectEntity is null)
            {
                _logger.LogError("Project with id: {0} was not found", projectId);
                return "Project was not found";
            }

            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            try
            {
                var reportsReponse = await pbiClient.Reports.GetReportsInGroupAsync(_workspaceId);

                var reports = new List<Report>();

                foreach (var r in reportsReponse.Value)
                {
                    if (r.Name.StartsWith(projectEntity.Name))
                    {
                        var entityInDb = await _dbContext.ProjectReports.FindAsync(r.Id);

                        if (entityInDb is not null) continue;

                        reports.Add(r);

                        var report = new ProjectReport()
                        {
                            PowerBiId = r.Id,
                            Name = r.Name.Substring(projectEntity.Name.Length + 1),
                            WorkspaceId = _workspaceId,
                            Project = projectEntity
                        };

                        await _dbContext.ProjectReports.AddAsync(report);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update reports for project id: {0}", projectId);
                return "Could not update reports for project";
            }

            return string.Empty;
        }
        public async Task<string> UpdateDashboardsAsync(int dashboardId)
        {
            var projectEntity = await _dbContext.Projects.FindAsync(dashboardId);

            if (projectEntity is null)
            {
                _logger.LogError("Project with id: {0} was not found", dashboardId);
                return "Project was not found";
            }

            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            try
            {
                var dashboradsResponse = await pbiClient.Dashboards.GetDashboardsInGroupAsync(_workspaceId);

                var dashboards = new List<Dashboard>();

                foreach (var d in dashboradsResponse.Value)
                {
                    if (d.DisplayName.StartsWith(projectEntity.Name))
                    {
                        var entityInDb = await _dbContext.ProjectDashboards.FindAsync(d.Id);

                        if (entityInDb is not null) continue;

                        dashboards.Add(d);

                        var dashboard = new ProjectDashboard()
                        {
                            PowerBiId = d.Id,
                            Name = d.DisplayName.Substring(projectEntity.Name.Length + 1),
                            WorkspaceId = _workspaceId,
                            Project = projectEntity
                        };

                        await _dbContext.ProjectDashboards.AddAsync(dashboard);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update dashboards for project id: {0}", dashboardId);
                return "Could not update dashboards for project";
            }

            return string.Empty;
        }
        public async Task UploadMetric(PBIDataset dataset, MetricPortion metric)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var datasets = await pbiClient.Datasets.GetDatasetsInGroupAsync(_workspaceId);

            var datasetName = $"{dataset.MetricFilesId}_{metric.Name}";

            string datasetId = string.Empty;
            foreach (var d in datasets.Value)
            {
                if (d.Name == datasetName)
                {
                    datasetId = d.Id;

                    break;
                }
            }

            if (string.IsNullOrEmpty(datasetId)) // Create new dataset
            {
                var dt = await CreateMetricDataset(dataset, metric);
                if (dt is null)
                {
                    return;
                }
                datasetId = dt.Id;
            }

            //Add rows
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

                var entityDataset = await _dbContext.Datasets.FindAsync(dataset.Id);
                entityDataset!.LastUpdate = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
            catch (Microsoft.Rest.HttpOperationException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
        }
        public async Task UploadMetric(PBIDataset dataset, List<MetricPortion> metrics)
        {
            foreach (var metric in metrics)
            {
                await UploadMetric(dataset, metric);
            }
        }
        private async Task<Dataset?> CreateMetricDataset(PBIDataset dataset, MetricPortion metric)
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

            var pushDatasetRequest = new CreateDatasetRequest($"{dataset.MetricFilesId}_{metric.Name}", tables);

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
                _ => throw new NotImplementedException(),
            };
        }
        private async Task<EmbedToken> GetEmbedReportToken(Guid reportId, Guid datasetId, Guid workspaceId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            /*var tokenRequest = new GenerateTokenRequestV2(
                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },
                datasets: new List<GenerateTokenRequestV2Dataset>() { new GenerateTokenRequestV2Dataset(datasetId.ToString()) },
                targetWorkspaces: new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(workspaceId) }
            );*/

            var tokenRequest = new GenerateTokenRequest(
                accessLevel: TokenAccessLevel.View
            );

            // Generate Embed token
            var embedToken = await pbiClient.Reports.GenerateTokenInGroupAsync(workspaceId, reportId, tokenRequest);

            return embedToken;
        }
        private async Task<EmbedToken> GetEmbedDashboardTokenAsync(Guid dashboardId, Guid workspaceId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            // Create a request for getting Embed token 
            var tokenRequest = new GenerateTokenRequest();

            // Generate Embed token
            var embedToken = await pbiClient.Dashboards.GenerateTokenInGroupAsync(workspaceId, dashboardId, tokenRequest);

            return embedToken;
        }
    }
}
