using System.Net;
using Blazored.Toast.Services;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Client.Pages.Projects;
using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Client.Utilities.Interfaces;
using PowerBiWeb.Shared.Project;
using System.Net.Http;
using System.Net.Http.Json;

namespace PowerBiWeb.Client.Utilities.Services
{
    public class ProjectService : IProjectService
    {
        private HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponse<ProjectDTO[]?>> GetProjectsAsync()
        {
            var response = await _httpClient.GetAsync("api/projects");
            if (response.IsSuccessStatusCode)
            {
                var projects = await response.Content.ReadFromJsonAsync<ProjectDTO[]>();
                return new()
                {
                    IsSuccess = true,
                    Value = projects,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Value = null
            };
        }
        public async Task<HttpResponse<ProjectDTO?>> GetProjectDetailAsync(int projectId)
        {
            var response = await _httpClient.GetAsync($"api/projects/{projectId}");

            if (response.IsSuccessStatusCode)
            {
                var project = await response.Content.ReadFromJsonAsync<ProjectDTO>();
                return new()
                {
                    IsSuccess = true,
                    Value = project,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Value = null
            };
        }
        public async Task<HttpResponse<ProjectDTO?>> CreateProject(ProjectDTO project)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", project);
            if (response.IsSuccessStatusCode)
            {
                var p = await response.Content.ReadFromJsonAsync<ProjectDTO>();
                return new()
                {
                    IsSuccess = true,
                    Value = p,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Value = null
            };
        }
        public async Task<HttpResponse> DeleteProject(int projectId)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> AddUserToProject(UserToProjectDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects/user", dto);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> EditUserInProject(UserToProjectDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/user", dto);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> RemoveUserFromProject(int projectId, int userId)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> EditProjectSettings(ProjectDTO project)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{project.Id}", project);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> UpdateReportsForProject(int projectId)
        {
            var response = await _httpClient.GetAsync($"api/reports/{projectId}/update");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> UpdateDashboardsForProject(int projectId)
        {
            var response = await _httpClient.GetAsync($"api/dashboards/{projectId}/update");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> AddPowerBiReport(int projectId, DashboardDTO report)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects/{projectId}/report", report);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> AddPowerBiDashboard(int projectId, DashboardDTO dashboard)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects/{projectId}/dashboard", dashboard);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> RemoveReportFromProject(int projectId, Guid reportId)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/report/{reportId}");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
        public async Task<HttpResponse> RemoveDashboardFromProject(int projectId, Guid dashboardId)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/dashboard/{dashboardId}");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    IsSuccess = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                IsSuccess = false
            };
        }
    }
}

