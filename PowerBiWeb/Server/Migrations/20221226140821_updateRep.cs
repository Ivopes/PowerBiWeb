using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class updateRep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports",
                column: "PowerBiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectReports",
                table: "ProjectReports",
                columns: new[] { "PowerBiId", "ProjectId" });
        }
    }
}
