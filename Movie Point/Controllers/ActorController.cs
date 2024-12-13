using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using Movie_Point.Models;
using Movie_Point.Repository;
using Movie_Point.Repository.IRepository;

namespace Movie_Point.Controllers
{
    
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMovieRepository _movieRepository;
        public ActorController(IActorRepository actorRepository , IMovieRepository  MovieRepository)
        {
            _actorRepository = actorRepository;
            _movieRepository = MovieRepository;
        }
        public IActionResult Index()
        {
            //var Actors = _dbContext.Actors.ToList();
            var Actors = _actorRepository.Get().ToList();
            return View(model:Actors);
        }
        public IActionResult GetActor(int id)
        {
            var Actor = _actorRepository.GetOne(e=>e.Id == id );
            var movies = _movieRepository.Get(e => e.actorsMovies.Any(e => e.Actor.Id == id),
                includeProps: e => e.Include(e=> e.Cinema).Include(e => e.Category)
                .Include(e => e.actorsMovies).ThenInclude(e=>e.Actor));
                
                
                //Movies.Include(e => e.Category).Include(e => e.Cinema)
                //.Include(e => e.actorsMovies).ThenInclude(e => e.Actor)
                //.Where(e => e.actorsMovies.Any(e => e.Actor.Id == id)).ToList();
            ViewBag.movies= movies;
            return View(model: Actor);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile? file)
        {
            ModelState.Remove("ProfilePicture");
            ModelState.Remove("file");
            ModelState.Remove("movies");
            ModelState.Remove("actorsMovies");

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
                    actor.ProfilePicture = fileName;
                }

                _actorRepository.Create(actor);

                
                TempData["success"] = "Add product successfuly";

                return RedirectToAction("Index");
            }
            return View(actor);
        }

        public IActionResult Edit(int id)
        {
            var actor = _actorRepository.GetOne(e=>e.Id == id);
            return View(actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile? file)
        {
            ModelState.Remove("ProfilePicture");
            ModelState.Remove("file");
            ModelState.Remove("movies");
            ModelState.Remove("actorsMovies");

            var oldActor = _actorRepository.GetOne(e => e.Id == actor.Id, tracked: false);

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

                    // Delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldActor.ProfilePicture);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    actor.ProfilePicture = fileName;
                }
                else
                {
                    actor.ProfilePicture = oldActor.ProfilePicture;
                }

                _actorRepository.Alter(actor);

                TempData["success"] = "Update actor successfuly";

                return RedirectToAction("Index");
            }

            actor.ProfilePicture = actor.ProfilePicture;
            
            return View(actor);
        }

        public IActionResult Delete(int id)
        {
            var actor = _actorRepository.GetOne(e => e.Id == id);
            if (actor == null) return RedirectToAction("NotFoundPage", "Home");
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actor.ProfilePicture);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
            _actorRepository.Delete(actor);
            TempData["success"] = "deleted actor successfully";
            return RedirectToAction("index");
        }
    }



    
}
