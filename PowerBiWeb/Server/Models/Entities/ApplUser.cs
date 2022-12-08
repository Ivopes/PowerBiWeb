using System.ComponentModel.DataAnnotations.Schema;

namespace PowerBiWeb.Server.Models.Entities
{
    public class ApplUser
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        [NotMapped]
        public string Password { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Email { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public AppRoles AppRole { get; set; }

        // Other entities
        public ICollection<AppUserProject> AppUserProjects { get; set; } = new HashSet<AppUserProject>();

    }
}
