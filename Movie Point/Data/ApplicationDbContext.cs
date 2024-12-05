using Microsoft.EntityFrameworkCore;
using Movie_Point.Models;

namespace Movie_Point.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie> actorsMovies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MoviePoint;Integrated Security=True;TrustServerCertificate=True");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<ActorMovie>()
        //        .HasKey(e => new { e.ActorsId, e.MoviesId });
        //}
    }
}
