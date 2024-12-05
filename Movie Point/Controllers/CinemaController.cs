using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;

namespace Movie_Point.Controllers
{
    public class CinemaController : Controller
    {
        readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var cinemas = _dbContext.Cinemas.ToList();
            return View(cinemas);
        }

        public IActionResult GetCinemaMovies(string CinemaName)
        {
            ViewBag.cinema = CinemaName;
            var movies= _dbContext.Movies.Include(e=>e.Cinema).Include(e => e.Category)
                .Where(e=>e.Cinema.Name == CinemaName).ToList();
            if (movies != null)
            { 
                return View(model: movies);
            
            }
            return NotFound();
        }

        
    }
}
