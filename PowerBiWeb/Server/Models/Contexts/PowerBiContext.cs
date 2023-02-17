using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Models.Contexts
{
    public class PowerBiContext : DbContext
    {
        public DbSet<ApplUser> AppUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<AppUserProject> AppUserProjects { get; set; }
        public DbSet<ProjectReport> ProjectReports { get; set; }
        public DbSet<ProjectDashboard> ProjectDashboards { get; set; }
        public DbSet<PBIDataset> Datasets { get; set; }
        public PowerBiContext(DbContextOptions<PowerBiContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserProject>()
                .HasKey(a => new { a.AppUserId, a.ProjectId });

            modelBuilder.Entity<PBIDataset>()
                .Property(d => d.ColumnNames)
                .HasConversion<string>(
                   v => JsonConvert.SerializeObject(v),
                   v => JsonConvert.DeserializeObject<List<string>>(v)!
                );

            modelBuilder.Entity<PBIDataset>()
               .Property(d => d.ColumnTypes)
               .HasConversion<string>(
                  v => JsonConvert.SerializeObject(v),
                  v => JsonConvert.DeserializeObject<List<string>>(v)!
               );

            modelBuilder.Entity<PBIDataset>()
               .Property(d => d.Measures)
               .HasConversion<string>(
                  v => JsonConvert.SerializeObject(v),
                  v => JsonConvert.DeserializeObject<List<string>>(v)!
               );

            modelBuilder.Entity<PBIDataset>()
               .Property(d => d.MeasureDefinitions)
               .HasConversion<string>(
                  v => JsonConvert.SerializeObject(v),
                  v => JsonConvert.DeserializeObject<List<string>>(v)!
               );
        }
    }
}
