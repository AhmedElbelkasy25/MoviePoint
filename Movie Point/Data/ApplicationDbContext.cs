using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Models;
using Movie_Point.Models.ViewModels;

namespace Movie_Point.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie> actorsMovies { get; set; }
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base(options)
        {
        }
        public ApplicationDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MoviePoint;Integrated Security=True;TrustServerCertificate=True");
        }
        public DbSet<Movie_Point.Models.ViewModels.ChangePasswordVM> ChangePasswordVM { get; set; } = default!;
        public DbSet<Movie_Point.Models.ViewModels.LoginUserVM> LoginUserVM { get; set; } = default!;
        
        
    }
}
