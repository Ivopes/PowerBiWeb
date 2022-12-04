using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private static readonly List<Project> _projects = new();

        public Task<List<Project>> GetAsync()
        {
            return Task.FromResult(_projects);
        }

        public Task<Project> GetAsync(int id)
        {
            return Task.FromResult(_projects[id]);
        }

        public Task<Project> Post(Project project)
        {
            _projects.Add(project);

            return Task.FromResult(project);
        }
    }
}
