using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProjectNastaveni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreateDatasets",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DownloadContent",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PowerBiPrefix",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDatasets",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DownloadContent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PowerBiPrefix",
                table: "Projects");
        }
    }
}
