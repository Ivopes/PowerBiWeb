using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Shared;
using System.Security.Cryptography;
using System.Text;

namespace PowerBiWeb.Server.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly PowerBiContext _dbContext;

        public AppUserRepository(PowerBiContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ApplUser>> GetAllAsync()
        {
            return await _dbContext.AppUsers.Include(a => a.AppUserProjects).ToListAsync();
        }

        public async Task<string> PostAsync(ApplUser user)
        {
            string response = string.Empty;

            if (await _dbContext.AppUsers.AnyAsync(u => u.Email == user.Email))
            {
                response = "Email is already in use";
                return response;
            }
            if (await _dbContext.AppUsers.AnyAsync(u => u.Username == user.Username))
            {
                response = "Username is already in use";
                return response;
            }

            user.Created= DateTime.UtcNow;

            user.PasswordSalt = PasswordUtility.GenerateSalt();

            user.PasswordHash = PasswordUtility.HashPassword(user.Password, user.PasswordSalt);

            await _dbContext.AppUsers.AddAsync(user);

            await SaveContextAsync();

            return response;
        }
        private async Task SaveContextAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
