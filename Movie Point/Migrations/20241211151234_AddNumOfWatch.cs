using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Point.Migrations
{
    /// <inheritdoc />
    public partial class AddNumOfWatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfWatch",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfWatch",
                table: "Movies");
        }
    }
}
