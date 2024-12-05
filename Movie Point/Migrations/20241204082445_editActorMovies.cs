using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie_Point.Migrations
{
    /// <inheritdoc />
    public partial class editActorMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "actorsMovies",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "ActorsId",
                table: "actorsMovies",
                newName: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_actorsMovies_ActorId",
                table: "actorsMovies",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_actorsMovies_MovieId",
                table: "actorsMovies",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_actorsMovies_Actors_ActorId",
                table: "actorsMovies",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_actorsMovies_Movies_MovieId",
                table: "actorsMovies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_actorsMovies_Actors_ActorId",
                table: "actorsMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_actorsMovies_Movies_MovieId",
                table: "actorsMovies");

            migrationBuilder.DropIndex(
                name: "IX_actorsMovies_ActorId",
                table: "actorsMovies");

            migrationBuilder.DropIndex(
                name: "IX_actorsMovies_MovieId",
                table: "actorsMovies");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "actorsMovies",
                newName: "MoviesId");

            migrationBuilder.RenameColumn(
                name: "ActorId",
                table: "actorsMovies",
                newName: "ActorsId");
        }
    }
}
