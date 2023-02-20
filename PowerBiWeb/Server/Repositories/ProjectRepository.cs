using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities;

namespace PowerBiWeb.Server.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PowerBiContext _dbContext;
        private readonly IMetricsApiLoaderRepository _metricsApiRepository;
        private readonly IMetricsContentRepository _metricsSaverRepository;
        public ProjectRepository(PowerBiContext dbContext, IMetricsApiLoaderRepository metricsApiRepository, IMetricsContentRepository metricsSaverRepository)
        {
            _dbContext = dbContext;
            _metricsApiRepository = metricsApiRepository;
            _metricsSaverRepository = metricsSaverRepository;
        }
        public async Task<List<Project>> GetAllAsync(int userId)
        {
            var result = await _dbContext.Projects
                .Where(p => p.AppUserProjects.Any(aup => aup.AppUser.Id == userId))
                .Include(p => p.AppUserProjects)
                .ToListAsync();

            return result;
        }
        public async Task<Project?> GetAsync(int id)
        {
            return await _dbContext.Projects
                .Include(p => p.AppUserProjects)
                .ThenInclude(aup => aup.AppUser)
                .Include(p => p.ProjectReports)
                .Include(p => p.ProjectDashboards)
                .SingleAsync(p => p.Id == id);
        }
        public async Task<Project> Post(int userId, Project project)
        {
            var user = await _dbContext.AppUsers.FindAsync(userId);

            user!.AppUserProjects.Add(new()
            {
                Project = project,
                Role = ProjectRoles.Creator
            });

            await SaveContextAsync();

            return project;
        }
        public async Task<string> AddToUserAsync(string userEmail, int projectId, ProjectRoles role)
        {
            var user = await _dbContext.AppUsers.SingleAsync(u => u.Email == userEmail);

            if (user is null) return "Email not found";

            var project = await _dbContext.Projects.FindAsync(projectId);

            if (project is null) return "Project not found";

            if (user.AppUserProjects.Any(aup => aup.ProjectId == projectId)) return "User is already assigned";

            user.AppUserProjects.Add(new()
            {
                Project = project,
                Role = role
            });

            try
            {
                await SaveContextAsync();
            }
            catch (UniqueConstraintException)
            {
                return "User is already assigned";

            }

            return string.Empty;
        }
        public async Task<string> EditUserAsync(string userEmail, int projectId, ProjectRoles newRole)
        {
            var entity = await _dbContext.AppUsers.SingleAsync(u => u.Email == userEmail);
            //var entity = await _dbContext.AppUsers.FindAsync(user.em);

            if (entity is null) return "User was not found";

            var aup = await _dbContext.AppUserProjects.Include(aup => aup.AppUser).SingleAsync(aup => aup.ProjectId == projectId && aup.AppUser.Email == userEmail);

            if (aup is null) return "User is not in specified project";

            aup.Role = newRole;

            await SaveContextAsync();

            return string.Empty;
        }
        public async Task<string> EditProject(int projectId, Project newProject)
        {
            var entity = await _dbContext.Projects.FindAsync(projectId);

            if (entity is null) return $"Project with Id: {projectId} was not found";

            entity.Name = newProject.Name;

            await _dbContext.SaveChangesAsync();

            return string.Empty;
        }
        public async Task<string> RemoveUserAsync(int userId, int projectId)
        {
            var entity = await _dbContext.AppUsers.Include(u => u.AppUserProjects).SingleAsync(u => u.Id == userId);

            if (entity is null) return "User was not found";

            var aup = entity.AppUserProjects.Single(aup => aup.ProjectId == projectId);

            if (aup is null) return "User is not in specified project";

            _dbContext.AppUserProjects.Remove(aup);

            await SaveContextAsync();

            return string.Empty;
        }
        public async Task<string> RemoveProject(int projectId)
        {
            var entity = await _dbContext.Projects.FindAsync(projectId);
            if (entity is null) return "Project not found";
            _dbContext.Projects.Remove(entity);

            await SaveContextAsync();

            return string.Empty;
        }
        public async Task<string> RemoveReportsAsync(int projectId, Guid reportId)
        {
            var entity = await _dbContext.Projects.Include(p => p.ProjectReports).SingleOrDefaultAsync(p => p.Id == projectId);

            if (entity is null) return "Project not found";

            var report = entity.ProjectReports.SingleOrDefault(r => r.PowerBiId == reportId);

            if (report is not null)
            {
                entity.ProjectReports.Remove(report);

                await SaveContextAsync();
            }

            return string.Empty;
        }
        public async Task<string> RemoveDashboardsAsync(int projectId, Guid dashboardId)
        {
            var entity = await _dbContext.Projects.Include(p => p.ProjectDashboards).SingleOrDefaultAsync(p => p.Id == projectId);

            if (entity is null) return "Dashboard not found";

            var dashboard = entity.ProjectDashboards.SingleOrDefault(d => d.PowerBiId == dashboardId);

            if (dashboard is not null)
            {
                entity.ProjectDashboards.Remove(dashboard);

                await SaveContextAsync();
            }

            return string.Empty;
        }

        #region Private Methods
        private async Task SaveContextAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
