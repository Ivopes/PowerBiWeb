using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Models.Contexts
{
    public class PowerBiContext : DbContext
    {
        public DbSet<ApplUser> AppUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<AppUserProject> AppUserProjects { get; set; }

        public PowerBiContext(DbContextOptions<PowerBiContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserProject>()
                .HasKey(a => new { a.AppUserId, a.ProjectId });
        }
    }
}
