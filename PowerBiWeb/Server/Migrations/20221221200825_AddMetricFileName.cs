using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetricFilesName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetricFilesName",
                table: "Projects");
        }
    }
}
