using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAsync();
        Task<Project> GetAsync(int id);
        Task<Project> Post(Project project);
    }
}
