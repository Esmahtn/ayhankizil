using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ayhankizil.Migrations
{
    /// <inheritdoc />
    public partial class ResimAlaniEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResimYolu",
                table: "Paylasimlar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResimYolu",
                table: "Paylasimlar");
        }
    }
}
