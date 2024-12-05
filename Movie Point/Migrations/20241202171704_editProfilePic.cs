using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Point.Migrations
{
    /// <inheritdoc />
    public partial class editProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilrPicture",
                table: "Actors",
                newName: "ProfilePicture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "Actors",
                newName: "ProfilrPicture");
        }
    }
}
