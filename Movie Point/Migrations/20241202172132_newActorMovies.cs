using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Point.Migrations
{
    /// <inheritdoc />
    public partial class newActorMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_actorsMovies",
                table: "actorsMovies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "actorsMovies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_actorsMovies",
                table: "actorsMovies",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_actorsMovies",
                table: "actorsMovies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "actorsMovies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_actorsMovies",
                table: "actorsMovies",
                columns: new[] { "ActorsId", "MoviesId" });
        }
    }
}
