using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnumu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppRole",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppRole",
                table: "AppUsers");
        }
    }
}
