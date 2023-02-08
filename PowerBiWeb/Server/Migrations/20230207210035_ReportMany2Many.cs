using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReportMany2Many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_Projects_ProjectId",
                table: "ProjectReports");

            migrationBuilder.DropIndex(
                name: "IX_ProjectReports_ProjectId",
                table: "ProjectReports");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectReports");

            migrationBuilder.CreateTable(
                name: "ProjectProjectReport",
                columns: table => new
                {
                    ProjectReportsPowerBiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProjectReport", x => new { x.ProjectReportsPowerBiId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectProjectReport_ProjectReports_ProjectReportsPowerBiId",
                        column: x => x.ProjectReportsPowerBiId,
                        principalTable: "ProjectReports",
                        principalColumn: "PowerBiId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProjectReport_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProjectReport_ProjectsId",
                table: "ProjectProjectReport",
                column: "ProjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProjectReport");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReports_ProjectId",
                table: "ProjectReports",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_Projects_ProjectId",
                table: "ProjectReports",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
