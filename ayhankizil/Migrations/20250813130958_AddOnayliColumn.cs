using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ayhankizil.Migrations
{
    /// <inheritdoc />
    public partial class AddOnayliColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Onayli",
                table: "Paylasimlar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Onayli",
                table: "Paylasimlar");
        }
    }
}
