using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class DatasetSolo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PBIDatasetProject");

            migrationBuilder.DropColumn(
                name: "DownloadContent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PowerBiPrefix",
                table: "Projects");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "PBIDatasetProject",
                columns: table => new
                {
                    DatasetsId = table.Column<int>(type: "int", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PBIDatasetProject", x => new { x.DatasetsId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_PBIDatasetProject_Datasets_DatasetsId",
                        column: x => x.DatasetsId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PBIDatasetProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PBIDatasetProject_ProjectsId",
                table: "PBIDatasetProject",
                column: "ProjectsId");
        }
    }
}
