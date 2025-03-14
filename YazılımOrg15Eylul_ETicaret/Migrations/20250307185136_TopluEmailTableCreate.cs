using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YazılımOrg15Eylul_ETicaret.Migrations
{
    /// <inheritdoc />
    public partial class TopluEmailTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopluEmails",
                columns: table => new
                {
                    TopluEmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopluEmails", x => x.TopluEmailId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopluEmails");
        }
    }
}
