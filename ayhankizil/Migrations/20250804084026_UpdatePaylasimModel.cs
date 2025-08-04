using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ayhankizil.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaylasimModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Paylasimlar");

            migrationBuilder.AddColumn<string>(
                name: "Foto1",
                table: "Paylasimlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto2",
                table: "Paylasimlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto3",
                table: "Paylasimlar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto4",
                table: "Paylasimlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto1",
                table: "Paylasimlar");

            migrationBuilder.DropColumn(
                name: "Foto2",
                table: "Paylasimlar");

            migrationBuilder.DropColumn(
                name: "Foto3",
                table: "Paylasimlar");

            migrationBuilder.DropColumn(
                name: "Foto4",
                table: "Paylasimlar");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Paylasimlar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
