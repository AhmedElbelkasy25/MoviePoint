using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;

namespace Movie_Point.Controllers
{
    
    public class ActorController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var Actors = _dbContext.Actors.ToList();
            return View(model:Actors);
        }
        public IActionResult GetActor(int id)
        {
            var Actor = _dbContext.Actors.Find(id);
            var movies = _dbContext.Movies.Include(e=>e.Category).Include(e => e.Cinema)
                .Include(e => e.actorsMovies).ThenInclude(e => e.Actor)
                .Where(e => e.actorsMovies.Any(e=>e.Actor.Id==id)).ToList();
            ViewBag.movies= movies;
            return View(model: Actor);
        }

    }
}
