using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IProjectService
    {
        Task<HttpResponse> AddUserToProject(UserToProjectDTO dto);
        Task<HttpResponse<ProjectDTO?>> CreateProject(ProjectDTO project);
        Task<HttpResponse> DeleteProject(int projectId);
        Task<HttpResponse> EditUserInProject(UserToProjectDTO dto);
        Task<HttpResponse<ProjectDTO?>> GetProjectDetailAsync(int projectId);
        Task<HttpResponse<ProjectDTO[]?>> GetProjectsAsync();
        Task<HttpResponse> RemoveUserFromProject(int projectId, int userId);
        Task<HttpResponse> EditProjectSettings(ProjectDTO project);
        Task<HttpResponse> UpdateReportsForProject(int projectId);
        Task<HttpResponse> UpdateDashboardsForProject(int projectId);

    }
}