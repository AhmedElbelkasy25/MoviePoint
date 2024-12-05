using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;

namespace Movie_Point.Controllers
{
    public class CategoryController : Controller
    {
        readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var movie = _dbContext.Categories.ToList();

            return View(model:movie);
        }

        public IActionResult GetCategoryMovies(string CategoryName)
        {
            ViewBag.Category = CategoryName;
            var movies = _dbContext.Movies.Include(e => e.Cinema).Include(e => e.Category)
                .Where(e => e.Category.Name == CategoryName).ToList();
            if (movies != null)
            {
                return View(model: movies);

            }
            return NotFound();
        }
    }
}
