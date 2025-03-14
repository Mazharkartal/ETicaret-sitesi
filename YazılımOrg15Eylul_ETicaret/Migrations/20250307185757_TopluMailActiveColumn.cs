using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YazılımOrg15Eylul_ETicaret.Migrations
{
    /// <inheritdoc />
    public partial class TopluMailActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TopluEmails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "TopluEmails");
        }
    }
}
