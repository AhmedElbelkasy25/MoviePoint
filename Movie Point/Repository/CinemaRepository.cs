using Movie_Point.Data;
using Movie_Point.Models;
using Movie_Point.Repository.IRepository;
using Movie_Point.Repositroy;

namespace Movie_Point.Repository
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
