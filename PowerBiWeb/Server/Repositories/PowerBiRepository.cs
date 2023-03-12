using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using PowerBiWeb.Client.Pages.Datasets;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Models.Metrics;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Server.Utilities.PowerBI;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;
using System.Data;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace PowerBiWeb.Server.Repositories
{
    public class PowerBiRepository : IMetricsContentRepository
    {
        private readonly AadService _aadService;
        private readonly Guid _workspaceId;
        private readonly ILogger<PowerBiRepository> _logger;
        private readonly PowerBiContext _dbContext;
        private const string TableName = "table1";
        public PowerBiRepository(AadService aadService, ILogger<PowerBiRepository> logger, PowerBiContext dbContext)
        {
            _aadService = aadService;
            _workspaceId = Guid.Parse(_aadService.WorkspaceId);
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<ReportDTO> GetEmbededReportAsync(Guid reportId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var report = await pbiClient.Reports.GetReportInGroupAsync(_workspaceId, reportId);

            EmbedToken embedToken = await GetEmbedReportToken(reportId, new Guid(report.DatasetId), _workspaceId);

            return new()
            {
                PowerBiName = report.Name,
                Id = reportId,
                EmbedToken = embedToken.Token,
                EmbedUrl = report.EmbedUrl
            };
        }
        public async Task<DashboardDTO> GetEmbededDashboardAsync(Guid dashboardId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var dashboard = await pbiClient.Dashboards.GetDashboardInGroupAsync(_workspaceId, dashboardId);

            EmbedToken embedToken = await GetEmbedDashboardTokenAsync(dashboardId, _workspaceId);

            return new()
            {
                PowerBiName = dashboard.DisplayName,
                Id = dashboardId,
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

                foreach (var r in reportsReponse.Value)
                {
                    ProjectReport? reportToUpdate;
                    // Zkontrolovat jestli je report soucasti projektu. Pokud ano, tak jenom update udaju
                    reportToUpdate = projectEntity.ProjectReports.SingleOrDefault(report => report.PowerBiId == r.Id);
                    if (reportToUpdate is not null)
                    {
                        reportToUpdate.PowerBIName = r.Name;
                    }
                    // Zkontrolovat jestli je report novy podle jmena a pridat ho do projektu
                    else if (r.Name.StartsWith(projectEntity.Name))
                    {
                        reportToUpdate = await _dbContext.ProjectReports.FindAsync(r.Id);

                        if (reportToUpdate is not null)
                        {
                            reportToUpdate.PowerBIName = r.Name;
                            if (!reportToUpdate.Projects.Contains(projectEntity))
                            {
                                reportToUpdate.Projects.Add(projectEntity);
                            }
                        }
                        else
                        {
                            reportToUpdate = new ProjectReport()
                            {
                                PowerBiId = r.Id,
                                Name = r.Name.Substring(projectEntity.Name.Length + 1),
                                PowerBIName = r.Name,
                                WorkspaceId = _workspaceId,
                                Projects = new List<Project>() { projectEntity }
                            };
                            await _dbContext.ProjectReports.AddAsync(reportToUpdate);
                        }

                    }
                    if (reportToUpdate is not null)
                    {
                        //Check jestli zname dataset
                        var rGuid = Guid.Parse(r.DatasetId);
                        var datasetEntity = await _dbContext.Datasets.SingleOrDefaultAsync(d => d.PowerBiId == rGuid);
                        if (datasetEntity is not null)
                        {
                            reportToUpdate.Dataset = datasetEntity;
                            reportToUpdate.DatasetId = datasetEntity.Id;
                        }
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
        public async Task<string> AddReportsAsync(int projectId, ProjectReport report)
        {
            var projectEntity = await _dbContext.Projects.FindAsync(projectId);

            if (projectEntity is null)
            {
                _logger.LogError("Project with id: {0} was not found or you dont have a permision", projectId);
                return "Project was not found or you dont have the right permision";
            }

            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            try
            {
                var reportResponse = await pbiClient.Reports.GetReportInGroupAsync(_workspaceId, report.PowerBiId);

                var entityUpdate = await _dbContext.ProjectReports.FindAsync(reportResponse.Id);

                if (entityUpdate is not null)
                {
                    if (!entityUpdate.Projects.Contains(projectEntity))
                    {
                        entityUpdate.Projects.Add(projectEntity);
                    }
                    else
                    {
                        return "Content is already in project";
                    }

                    //a update udaju
                    entityUpdate.PowerBIName = reportResponse.Name;
                }
                else
                {
                    entityUpdate = new ProjectReport()
                    {
                        PowerBiId = reportResponse.Id,
                        PowerBIName = reportResponse.Name,
                        Name = report.Name,
                        WorkspaceId = _workspaceId,
                        Projects = new List<Project>() { projectEntity }
                    };

                    await _dbContext.ProjectReports.AddAsync(entityUpdate);
                }
                //Check jestli zname dataset
                var rGuid = Guid.Parse(reportResponse.DatasetId);
                var datasetEntity = await _dbContext.Datasets.SingleOrDefaultAsync(d => d.PowerBiId == rGuid);
                if (datasetEntity is not null) 
                {
                    entityUpdate.Dataset = datasetEntity;
                    entityUpdate.DatasetId = datasetEntity.Id;
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add report with id: {0} for project id: {1}",report.PowerBiId ,projectId);
                return "Could not add report for project";
            }

            return string.Empty;
        }
        public async Task<string> UpdateDashboardsAsync(int projectId)
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
                var dashboradsResponse = await pbiClient.Dashboards.GetDashboardsInGroupAsync(_workspaceId);


                foreach (var d in dashboradsResponse.Value)
                {
                    // Zkontrolovat jestli je dashboard soucasti projektu. Pokud ano, tak jenom update udaju
                    var dashboardToUpdate = projectEntity.ProjectDashboards.SingleOrDefault(dashboard => dashboard.PowerBiId == d.Id);
                    if (dashboardToUpdate is not null)
                    {
                        dashboardToUpdate.PowerBiName = d.DisplayName;
                    }
                    // Zkontrolovat jestli je dashboard novy podle jmena a pridat ho do projektu
                    else if (d.DisplayName.StartsWith(projectEntity.Name))
                    {
                        dashboardToUpdate = await _dbContext.ProjectDashboards.FindAsync(d.Id);

                        if (dashboardToUpdate is not null)
                        {
                            dashboardToUpdate.PowerBiName = d.DisplayName;
                            if (!dashboardToUpdate.Projects.Contains(projectEntity))
                            {
                                dashboardToUpdate.Projects.Add(projectEntity);
                            }
                        }
                        else
                        {
                            var dashboard = new ProjectDashboard()
                            {
                                PowerBiId = d.Id,
                                Name = d.DisplayName.Substring(projectEntity.Name.Length + 1),
                                PowerBiName = d.DisplayName,
                                WorkspaceId = _workspaceId,
                                Projects = new List<Project>() {projectEntity}
                            };

                            await _dbContext.ProjectDashboards.AddAsync(dashboard);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update dashboards for project id: {0}", projectId);
                return "Could not update dashboards for project";
            }

            return string.Empty;
        }
        public async Task<string> AddDashboardsAsync(int projectId, ProjectDashboard dashboard)
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
                var dashboardResponse = await pbiClient.Dashboards.GetDashboardInGroupAsync(_workspaceId, dashboard.PowerBiId);

                var entityInDb = await _dbContext.ProjectDashboards.FindAsync(dashboardResponse.Id);

                if (entityInDb is not null) return "Content is already in project";

                var entityCreated = new ProjectDashboard()
                {
                    PowerBiId = dashboardResponse.Id,
                    Name = dashboard.Name,
                    PowerBiName = dashboardResponse.DisplayName,
                    WorkspaceId = _workspaceId,
                    Projects = new List<Project>() {projectEntity}
                };

                await _dbContext.ProjectDashboards.AddAsync(entityCreated);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add dashboard with id: {0} for project id: {1}", dashboard.PowerBiId, projectId);
                return "Could not add dashboard for project";
            }

            return string.Empty;
        }
        public async Task<bool> AddRowsToDataset(PBIDataset dataset, MetricData data)
        {
            PostRowsRequest request = new PostRowsRequest
            {
                Rows = data.Rows.ToArray(),
            };

            try
            {
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
                
                // Must use HttpClient because power bi SDK cant handle custom serialization
                var httpClient = new HttpClient();
                var url = $"https://api.powerbi.com/v1.0/myorg/groups/{_workspaceId}/datasets/{dataset.PowerBiId}/tables/{TableName}/rows";

                httpClient.BaseAddress = new Uri(url);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _aadService.GetAccessToken());

                var response = await httpClient.PostAsync(string.Empty, content);
                if (!response.IsSuccessStatusCode)
                {
                    string r = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Could not post rows: {r}");
                    return false;
                }
                //var response = await pbiClient.Datasets.PostRowsInGroupWithHttpMessagesAsync(_workspaceId, dataset.PowerBiId.ToString(), TableName, request);
                //await pbiClient.Datasets.PostRowsInGroupAsync(_workspaceId, dataset.PowerBiId.ToString(), TableName, request);

                var entityDataset = await _dbContext.Datasets.FindAsync(dataset.Id);
                entityDataset!.LastUpdate = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (HttpOperationException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not push rows for dataset with id: {0}", dataset.Id);
            }

            return false;
        }
        public async Task<Dataset?> CreateDatasetFromDefinition(MetricDefinition definition)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var columns = new List<Column>();
            for (int i = 0; i < definition.ColumnNames.Count; i++)
            {
                columns.Add(new Column(definition.ColumnNames[i], definition.ColumnTypes[i]));
            }

            var measures = new List<Measure>();
            for (int i = 0; i < definition.Measures.Count; i++)
            {
                measures.Add(new Measure(definition.Measures[i], definition.MeasureDefinitions[i]));
            }

            var tables = new List<Table>();
            var table = new Table()
            {
                Name = "table1"
            };

            table.Columns = columns;
            table.Measures = measures;

            tables.Add(table);

            var pushDatasetRequest = new CreateDatasetRequest($"{definition.Name}", tables, defaultMode: DatasetMode.Push);

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
        public async Task<Report?> CloneReportAsync(Guid reportId, string reportNewName)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var request = new CloneReportRequest(reportNewName);

            var result = await pbiClient.Reports.CloneReportInGroupWithHttpMessagesAsync(_workspaceId, reportId, request);
            if (!result.Response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while rebinding report :{0}", await result.Response.Content.ReadAsStringAsync());
                return null;
            }

            return result.Body;
        }
        public async Task<bool> RebindReportAsync(Guid reportId, Guid datasetId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var request = new RebindReportRequest(datasetId.ToString());
            
            var result = await pbiClient.Reports.RebindReportInGroupWithHttpMessagesAsync(_workspaceId, reportId, request);

            if (!result.Response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while rebinding report :{0}", await result.Response.Content.ReadAsStringAsync());
                return false;
            }
            
            return true;
        }
        public async Task<Stream?> GetExportedReportAsync(Guid reportId)
        {
            PowerBIClient pbiClient = PowerBiUtility.GetPowerBIClient(_aadService);

            var request = new ExportReportRequest
            {
                Format = FileFormat.PPTX
            };

            var result = await pbiClient.Reports.ExportToFileInGroupWithHttpMessagesAsync(_workspaceId, reportId, request);

            if (result.Response.IsSuccessStatusCode)
            {
                do
                {
                    await Task.Delay(100);
                    result = await pbiClient.Reports.GetExportToFileStatusInGroupWithHttpMessagesAsync(_workspaceId, reportId, result.Body.Id);

                } while (result.Body.Status == ExportState.Running && result.Response.IsSuccessStatusCode);

                if (result.Response.IsSuccessStatusCode && result.Body.Status == ExportState.Succeeded)
                {
                    var streamResult = await pbiClient.Reports.GetFileOfExportToFileInGroupWithHttpMessagesAsync(_workspaceId, reportId, result.Body.Id);
                    if (streamResult.Response.IsSuccessStatusCode)
                    {
                        return streamResult.Body;
                    }
                }

            }
            
            _logger.LogError("Error while exporting report :{0}", await result.Response.Content.ReadAsStringAsync());
            return null;
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
