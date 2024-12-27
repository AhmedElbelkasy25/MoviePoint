using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Point.Migrations
{
    /// <inheritdoc />
    public partial class addIsBlockedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ISBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBlocked",
                table: "AspNetUsers");
        }
    }
}
