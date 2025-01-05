using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using Movie_Point.Repository.IRepository;
using Movie_Point.Models;
using Movie_Point.Repository;
using Microsoft.AspNetCore.Authorization;
namespace Movie_Point.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieRepository _movieRepository;
        public CategoryController(ICategoryRepository categoryRepository , IMovieRepository movieRepository)
        {
            _categoryRepository = categoryRepository;
            _movieRepository = movieRepository;
        }
        
        public IActionResult Index()
        {
            var categories = _categoryRepository.Get().ToList();

            return View(model:categories);
        }
        [AllowAnonymous]
        public IActionResult GetCategoryMovies(string CategoryName)
        {
            ViewBag.Category = CategoryName;
            var movies = _movieRepository.Get(e => e.Category.Name == CategoryName,
                includeProps: e => e.Include(e => e.Cinema).Include(e => e.Category)).ToList();
                
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
        public IActionResult Create(Category category)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                _categoryRepository.Create(category);

                TempData["success"] = "Add Category successfuly";
                return RedirectToAction("index");
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if (category != null)
            {
                return View(category);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            ModelState.Remove("Movies");
            if (ModelState.IsValid)
            {
                _categoryRepository.Alter(category);
                TempData["success"] = "Update Category successfuly";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            
            var category = _categoryRepository.GetOne(e => e.Id == id);
            if (category != null)
            {
                _categoryRepository.Delete(category);
                _categoryRepository.Saving();   

                TempData["success"] = "Delete Category successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}
