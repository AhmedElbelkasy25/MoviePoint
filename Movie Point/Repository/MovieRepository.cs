using Movie_Point.Repository.IRepository;
using Movie_Point.Repositroy;
using Movie_Point.Models;
using Movie_Point.Data;

namespace Movie_Point.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
