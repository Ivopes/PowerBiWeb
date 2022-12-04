using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAsync();
        Task<Project> GetAsync(int id);
        Task<Project> PostAsync(Project project);
    }
}