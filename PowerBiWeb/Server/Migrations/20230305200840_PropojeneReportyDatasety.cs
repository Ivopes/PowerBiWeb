using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class PropojeneReportyDatasety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DatasetId",
                table: "ProjectReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReports_DatasetId",
                table: "ProjectReports",
                column: "DatasetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports",
                column: "DatasetId",
                principalTable: "Datasets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports");

            migrationBuilder.DropIndex(
                name: "IX_ProjectReports_DatasetId",
                table: "ProjectReports");

            migrationBuilder.DropColumn(
                name: "DatasetId",
                table: "ProjectReports");
        }
    }
}
