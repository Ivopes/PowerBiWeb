using Blazored.Toast.Services;
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
                    Value = projects,
                    ErrorMessage = string.Empty
                };
            };
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
                    Value = project,
                    ErrorMessage = string.Empty
                };
            };
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
                    Success = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Success = false
            };
        }
        public async Task<HttpResponse> AddUserToProject(UserToProjectDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/projects/user", dto);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    Success = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Success = false
            };
        }
        public async Task<HttpResponse> EditUserInProject(UserToProjectDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/user", dto);

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    Success = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Success = false
            };
        }
        public async Task<HttpResponse> RemoveUserFromProject(int projectId, int userId)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{projectId}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return new()
                {
                    Success = true,
                    ErrorMessage = string.Empty
                };
            }
            return new()
            {
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Success = false
            };
        }
    }
}
}
