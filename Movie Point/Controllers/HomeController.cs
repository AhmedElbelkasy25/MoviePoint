using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using Movie_Point.Models;
using Movie_Point.Models.ViewModels;
using Movie_Point.Repository.IRepository;

namespace Movie_Point.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository _movieRepository;
        public HomeController(ILogger<HomeController> logger , IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;

        }

        public IActionResult Index(int pageNumber=1 , int numOfItems = 9)
        {
            var movies = _movieRepository.Get(includeProps:e=>e.Include(e => e.Cinema)
            .Include(e => e.Category));
            foreach(var item in movies)
            {   
                if(item.StartDate > DateTime.Now)
                    item.MovieStatus = Data.Enums.MovieStatus.Upcoming;
                else if(item.StartDate< DateTime.Now && item.EndDate >  DateTime.Now)
                    item.MovieStatus = Data.Enums.MovieStatus.Available;
                else
                    item.MovieStatus = Data.Enums.MovieStatus.Expired;
            }
            _movieRepository.Saving();

            int pages = (int)Math.Ceiling((double)movies.Count() / numOfItems);
            movies = movies.Skip((pageNumber -1) * numOfItems).Take(numOfItems);
            moviePaginationVM paginationVM = new moviePaginationVM() {
            Movies=movies.ToList(),
            Pages=pages,
            PageNumber=pageNumber
            };
            
            return View(paginationVM);
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
