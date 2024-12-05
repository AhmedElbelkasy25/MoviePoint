using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;

namespace Movie_Point.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index),"Home");
        }
        public IActionResult Details(int id)
        {
            var movie = _dbContext.Movies.Include(e => e.Cinema).
                Include(e => e.Category).Include(e => e.actorsMovies).
                ThenInclude(a=>a.Actor).FirstOrDefault(e => e.Id == id);
            return View(movie);
        }
        [HttpPost]
        public IActionResult SearchForMovie(string Name)
        {
            var movies = _dbContext.Movies.Include(e => e.Cinema).
                Include(e => e.Category).Where(e => e.Name.Contains(Name)).ToList();
            if (movies != null)
                return View(movies);
            return RedirectToAction("NotFoundPage" , "Home");
        }

    }
}
