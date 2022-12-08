using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly PowerBiContext _dbContext;

        public AppUserRepository(PowerBiContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<AppUser>> GetAsync()
        {
            return await _dbContext.AppUsers.Include(a => a.AppUserProjects).ToListAsync();
        }
    }
}
