using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YazılımOrg15Eylul_ETicaret.Migrations
{
    /// <inheritdoc />
    public partial class Puanlama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrtlamaPuan",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OySayisi",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToplamPuan",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Puan",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrtlamaPuan",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OySayisi",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ToplamPuan",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Puan",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddDate",
                table: "Comments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
