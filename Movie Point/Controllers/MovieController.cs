using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Models;
using Movie_Point.Repository.IRepository;

namespace Movie_Point.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MovieController : Controller
    {
        //private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        private readonly IMovieRepository _movieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IActorMovieRepository _actorMovieRepository;
        public MovieController(IMovieRepository movieRepository , ICategoryRepository categoryRepository
            , ICinemaRepository cinemaRepository, IActorRepository actorRepository
            , IActorMovieRepository actorMovieRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
            _actorRepository = actorRepository;
            _actorMovieRepository = actorMovieRepository;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var movies = _movieRepository.Get(includeProps: e => e.Include(e => e.Cinema).Include(e => e.Category)).ToList();
            return View(movies);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            
            var movie = _movieRepository.GetOne(filter: e => e.Id == id, includeProps: e => e.Include(e => e.Cinema)
                .Include(e => e.Category).Include(e => e.actorsMovies).ThenInclude(e => e.Actor));
            movie.NumOfWatch++;
            _movieRepository.Saving();
            return View(movie);
        }
        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult SearchForMovie(string name)
        //{
        //    var movies = _movieRepository.Get(
        //        e => e.Name.Contains(name),
        //        includeProps: e => e.Include(e => e.Cinema).Include(e => e.Category)
        //    ).ToList();

        //    if (movies != null && movies.Any())
        //    {
        //        // Return movie details as JSON
        //        return Json(new { success = true, data = movies });
        //    }

        //    return Json(new { success = false, message = "No movies found." });
        //}

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SearchForMovie(string Name)
        {
            var movies = _movieRepository.Get(e => e.Name.Contains(Name), includeProps: e => e.Include(e => e.Cinema).Include(e => e.Category)).ToList();
            if (movies.Count !=0)
                return View(movies);
            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult Create()
        {
            var categories = _categoryRepository.Get().ToList();
            var cinemas = _cinemaRepository.Get().ToList();
            var actors = _actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;
            return View(new Movie());
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? file , int[] ActorId)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("actorsMovies");
            ModelState.Remove("ImgUrl");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save in db
                    movie.ImgUrl = fileName;
                }
                _movieRepository.Create(movie);
                var lastMovie= _movieRepository.Get().OrderByDescending(x => x.Id).FirstOrDefault();
                for (int i = 0; i < ActorId.Length; i++)
                {
                    _actorMovieRepository.Create(new ActorMovie { MovieId = lastMovie.Id, ActorId = ActorId[i] });
                }
                TempData["success"] = "Add Movie successfully";
                return RedirectToAction("index");

            }
            var categories = _categoryRepository.Get().ToList();
            var cinemas = _cinemaRepository.Get().ToList();
            var actors = _actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie= _movieRepository.GetOne(e=>e.Id== id);
            if (movie == null) return RedirectToAction("NotFoundPage", "Home");
            var categories = _categoryRepository.Get().ToList();
            var cinemas = _cinemaRepository.Get().ToList();
            var actors = _actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;

            return View(model:movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile? file, int[] ActorId)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("Actors");
            ModelState.Remove("actorsMovies");
            ModelState.Remove("ImgUrl");

            var oldMovie = _movieRepository.GetOne(e => e.Id == movie.Id, tracked: false);
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    //delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movis", oldMovie.ImgUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    // Save in db
                    movie.ImgUrl = fileName;
                }
                else
                {
                    movie.ImgUrl = oldMovie.ImgUrl;
                }
                _movieRepository.Alter(movie);
                TempData["success"] = "Edit Movie successfully";
                var actorMovie =_actorMovieRepository.Get(e => e.MovieId == movie.Id).ToList();
                foreach(var item in actorMovie)
                {
                    _actorMovieRepository.Delete(item);
                }

                for (int i = 0; i < ActorId.Length; i++)
                {
                    _actorMovieRepository.Create(new ActorMovie { MovieId = movie.Id, ActorId = ActorId[i] });
                }

                _actorMovieRepository.Saving();
                
                
                return RedirectToAction("index");

            }
            var categories = _categoryRepository.Get().ToList();
            var cinemas = _cinemaRepository.Get().ToList();
            var actors = _actorRepository.Get().ToList();
            ViewBag.Category = categories;
            ViewBag.Cinema = cinemas;
            ViewBag.Actor = actors;
            return View(movie);
        }
        public IActionResult Delete(int id)
        {
            var movie = _movieRepository.GetOne(e => e.Id == id);

            if (movie == null) return RedirectToAction("NotFoundPage", "Home");

            // Delete old img
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movie.ImgUrl);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _movieRepository.Delete(movie);

            TempData["success"] = "Delete Movie successfully";

            return RedirectToAction("Index");
        }

        
    }


    
}
