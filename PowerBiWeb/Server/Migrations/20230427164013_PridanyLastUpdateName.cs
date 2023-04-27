using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class PridanyLastUpdateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdateName",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateName",
                table: "Datasets");
        }
    }
}
