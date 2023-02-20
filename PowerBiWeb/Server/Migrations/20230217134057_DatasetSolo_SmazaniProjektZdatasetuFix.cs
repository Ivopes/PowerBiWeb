using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class DatasetSoloSmazaniProjektZdatasetuFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Datasets_PBIDatasetId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PBIDatasetId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PBIDatasetId",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PBIDatasetId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PBIDatasetId",
                table: "Projects",
                column: "PBIDatasetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Datasets_PBIDatasetId",
                table: "Projects",
                column: "PBIDatasetId",
                principalTable: "Datasets",
                principalColumn: "Id");
        }
    }
}
