﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerBiWeb.Server.Models.Contexts;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    [DbContext(typeof(PowerBiContext))]
    [Migration("20230204194844_ProjectNastaveni")]
    partial class ProjectNastaveni
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CreateDatasets")
                        .HasColumnType("bit");

                    b.Property<bool>("DownloadContent")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetricFilesName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PowerBiPrefix")
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

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PowerBiId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectDashboards");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectReport", b =>
                {
                    b.Property<Guid>("PowerBiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PowerBiId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectReports");
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

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectDashboard", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.Project", "Project")
                        .WithMany("ProjectDashboards")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ProjectReport", b =>
                {
                    b.HasOne("PowerBiWeb.Server.Models.Entities.Project", "Project")
                        .WithMany("ProjectReports")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.ApplUser", b =>
                {
                    b.Navigation("AppUserProjects");
                });

            modelBuilder.Entity("PowerBiWeb.Server.Models.Entities.Project", b =>
                {
                    b.Navigation("AppUserProjects");

                    b.Navigation("ProjectDashboards");

                    b.Navigation("ProjectReports");
                });
#pragma warning restore 612, 618
        }
    }
}
