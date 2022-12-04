using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetAsync()
        {
            return await _projectRepository.GetAsync();
        }

        public async Task<Project> GetAsync(int id)
        {
            return await _projectRepository.GetAsync(id);

        }

        public async Task<Project> PostAsync(Project project)
        {
            return await _projectRepository.Post(project);  
        }
    }
}
