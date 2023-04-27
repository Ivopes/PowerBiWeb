using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities;
using PowerBiWeb.Shared.Users;

namespace PowerBiWeb.Server.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PowerBiContext _dbContext;

        public AuthRepository(PowerBiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplUser?> LoginAsync(UserLoginInformation user)
        {
            string token = string.Empty;

            var entity = await _dbContext.AppUsers.SingleOrDefaultAsync(u => u.Username == user.Username);

            if (entity is null) return null;


            byte[] passHash = PasswordUtility.HashPassword(user.Password, entity.PasswordSalt);

            if (!Enumerable.SequenceEqual(entity.PasswordHash, passHash)) return null;

            return entity;
        }
    }
}
