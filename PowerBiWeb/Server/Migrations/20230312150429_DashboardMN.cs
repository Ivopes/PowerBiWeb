using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class DashboardMN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDashboards_Projects_ProjectId",
                table: "ProjectDashboards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDashboards_ProjectId",
                table: "ProjectDashboards");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectDashboards");

            migrationBuilder.CreateTable(
                name: "ProjectProjectDashboard",
                columns: table => new
                {
                    ProjectDashboardsPowerBiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProjectDashboard", x => new { x.ProjectDashboardsPowerBiId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectProjectDashboard_ProjectDashboards_ProjectDashboardsPowerBiId",
                        column: x => x.ProjectDashboardsPowerBiId,
                        principalTable: "ProjectDashboards",
                        principalColumn: "PowerBiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProjectDashboard_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProjectDashboard_ProjectsId",
                table: "ProjectProjectDashboard",
                column: "ProjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProjectDashboard");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectDashboards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDashboards_ProjectId",
                table: "ProjectDashboards",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDashboards_Projects_ProjectId",
                table: "ProjectDashboards",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
