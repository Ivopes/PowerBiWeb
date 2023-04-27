using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class kaskadoveMazaniDatasetuPryc2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports",
                column: "DatasetId",
                principalTable: "Datasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReports_Datasets_DatasetId",
                table: "ProjectReports",
                column: "DatasetId",
                principalTable: "Datasets",
                principalColumn: "Id");
        }
    }
}
