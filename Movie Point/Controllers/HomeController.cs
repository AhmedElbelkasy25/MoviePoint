using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using Movie_Point.Models;

namespace Movie_Point.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movie = _dbContext.Movies.Include(e=>e.Cinema).Include(e=>e.Category).ToList();
            foreach(var item in movie)
            {   
                if(item.StartDate > DateTime.Now)
                    item.MovieStatus = Data.Enums.MovieStatus.Upcoming;
                else if(item.StartDate< DateTime.Now && item.EndDate >  DateTime.Now)
                    item.MovieStatus = Data.Enums.MovieStatus.Available;
                else
                    item.MovieStatus = Data.Enums.MovieStatus.Expired;
            }
            _dbContext.SaveChanges();
            return View(movie);
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
