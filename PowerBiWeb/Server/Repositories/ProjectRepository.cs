﻿using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PowerBiContext _dbContext;

        public ProjectRepository(PowerBiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllAsync(int userId)
        {
            var result = await _dbContext.Projects
                .Where(p => p.AppUserProject.Any(aup => aup.AppUser.Id == userId))
                .Include(p => p.AppUserProject)
                .ToListAsync();

            return result;
        }

        public async Task<Project?> GetAsync(int id)
        {
            return await _dbContext.Projects.FindAsync(id);
        }

        public async Task<Project> Post(int userId, Project project)
        {
            var user = await _dbContext.AppUsers.FindAsync(userId);

            user!.AppUserProjects.Add(new() 
            { 
                Project = project,
                Role = ProjectRoles.Creator
            });

            //await _dbContext.Projects.AddAsync(project);
            await SaveContextAsync();

            return project;
        }
        public async Task<string> AddToUser(string userEmail, int projectId, ProjectRoles role)
        {
            var user = await _dbContext.AppUsers.SingleAsync(u => u.Email == userEmail);
            
            if (user is null) return "Email not found";

            var project = await _dbContext.Projects.FindAsync(projectId);

            if (project is null) return "Project not found";

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
        private async Task SaveContextAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
