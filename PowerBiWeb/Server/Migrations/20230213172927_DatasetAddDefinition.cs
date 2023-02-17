using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerBiWeb.Server.Migrations
{
    /// <inheritdoc />
    public partial class DatasetAddDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnNames",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColumnTypes",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeasureDefinitions",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Measures",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Datasets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PowerBiId",
                table: "Datasets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnNames",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "ColumnTypes",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "MeasureDefinitions",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "Measures",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "PowerBiId",
                table: "Datasets");
        }
    }
}
