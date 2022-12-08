using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PowerBiWeb.Server.Models.Entities
{
    public class AppUserProject
    {
        [Key, Column(Order = 0)]
        public int AppUserId { get; set; }
        [Key, Column(Order = 1)]
        public int ProjectId { get; set; }

        public virtual ApplUser AppUser { get; set; } = new();
        public virtual Project Project { get; set; } = new();
        public ProjectRoles Role { get; set; }
    }
}
