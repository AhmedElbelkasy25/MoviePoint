using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using Movie_Point.Models;
using Movie_Point.Repository;
using Movie_Point.Repository.IRepository;

namespace Movie_Point.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMovieRepository _movieRpository;

        public CinemaController(ICinemaRepository cinemaRepository, IMovieRepository movieRpository)
        {
            _cinemaRepository = cinemaRepository;
            _movieRpository = movieRpository;
        }
        [Authorize]
        public IActionResult Index()
        {
            var cinemas = _cinemaRepository.Get().ToList();
            return View(cinemas);
        }
        [Authorize]
        public IActionResult GetCinemaMovies(string CinemaName)
        {
            ViewBag.cinema = CinemaName;
            var movies= _movieRpository.Get(filter:e => e.Cinema.Name == CinemaName , includeProps:
                e=>e.Include(e => e.Cinema).Include(e => e.Category)).ToList();

            if (movies != null)
            { 
                return View(model: movies);
            
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                _cinemaRepository.Create(cinema);

                TempData["success"] = "Add Cinema successfuly";
                return RedirectToAction("index");
            }
            return View(cinema);
        }
        public IActionResult Edit(int id)
        {

            var cinema = _cinemaRepository.GetOne(e => e.Id == id);

            if (cinema != null)
            {
                return View(cinema);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                _cinemaRepository.Alter(cinema);
                TempData["success"] = "Update cinema successfuly";
                return RedirectToAction("Index");
            }

            return View(cinema);
        }
        public IActionResult Delete(int id)
        {

            var cinema = _cinemaRepository.GetOne(e => e.Id == id);
            if (cinema != null)
            {
                _cinemaRepository.Delete(cinema);

                TempData["success"] = "Delete cinema successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }


    }
}
