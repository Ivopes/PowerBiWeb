﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerBiWeb.Server.Models.Contexts;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    [DbContext(typeof(PowerBiContext))]
    partial class PowerBiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.AppUserProject", b =>
                {
                    b.Property<int>("AppUserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("AppUserId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("AppUserProjects");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ApplUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppRole")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.PBIDataset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ColumnNames")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ColumnTypes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MeasureDefinitions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Measures")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetricFilesId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PowerBiId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Datasets");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectDashboard", b =>
                {
                    b.Property<Guid>("PowerBiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PowerBiName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PowerBiId");

                    b.ToTable("ProjectDashboards");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectReport", b =>
                {
                    b.Property<Guid>("PowerBiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("DatasetId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PowerBIName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PowerBiId");

                    b.HasIndex("DatasetId");

                    b.ToTable("ProjectReports");
                });

            modelBuilder.Entity("ProjectProjectDashboard", b =>
                {
                    b.Property<Guid>("ProjectDashboardsPowerBiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.HasKey("ProjectDashboardsPowerBiId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("ProjectProjectDashboard");
                });

            modelBuilder.Entity("ProjectProjectReport", b =>
                {
                    b.Property<Guid>("ProjectReportsPowerBiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.HasKey("ProjectReportsPowerBiId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("ProjectProjectReport");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.AppUserProject", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.ApplUser", "AppUser")
                        .WithMany("AppUserProjects")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PowerBiWeb.Server.Models.Entities.Project", "Project")
                        .WithMany("AppUserProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectReport", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.PBIDataset", "Dataset")
                        .WithMany("Reports")
                        .HasForeignKey("DatasetId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Dataset");
                });

            modelBuilder.Entity("ProjectProjectDashboard", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.ProjectDashboard", null)
                        .WithMany()
                        .HasForeignKey("ProjectDashboardsPowerBiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PowerBiWeb.Server.Models.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectProjectReport", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.ProjectReport", null)
                        .WithMany()
                        .HasForeignKey("ProjectReportsPowerBiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PowerBiWeb.Server.Models.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ApplUser", b =>
                {
                    b.Navigation("AppUserProjects");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.PBIDataset", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.Project", b =>
                {
                    b.Navigation("AppUserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
