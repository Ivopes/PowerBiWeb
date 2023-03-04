using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Server.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<string> ChangeUsernameAsync(int userId, string newUsername)
        {
            return await _appUserRepository.ChangeUsernameAsync(userId, newUsername);
        }

        public async Task<IEnumerable<ApplUser>> GetAsync()
        {
            return await _appUserRepository.GetAllAsync();
        }

        public async Task<UserDetail?> GetByIdAsync(int id)
        {
            var entity = await _appUserRepository.GetByIdAsync(id);

            if (entity is null)
            {
                return null;
            }

            var result = new UserDetail
            {
                Email = entity.Email,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                Username = entity.Username,
            };

            return result;
        }

        public async Task<string> PostAsync(UserRegisterInformation user)
        {
            ApplUser u = new()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
                AppRole = AppRoles.User
            };
            return await _appUserRepository.PostAsync(u);
        }
    }
}
