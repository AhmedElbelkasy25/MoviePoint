using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Point.Models;
using Movie_Point.Repository;
using Movie_Point.Repository.IRepository;
using Stripe;

namespace Movie_Point.Controllers
{
    
    //[Authorize(Roles ="Admin")]
    public class RequestCinemaController : Controller
    {
        private readonly IRequestCinemaRepository _requestCinema;
        private readonly ICinemaRepository _cinemaRepository;

        public RequestCinemaController(IRequestCinemaRepository requestCinema , ICinemaRepository cinemaRepository)
        {
            this._requestCinema = requestCinema;
            this._cinemaRepository = cinemaRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var cinemas = _requestCinema.Get();

            return View(model: cinemas.ToList());
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(RequestCinema reqCinema)
        {
            if (ModelState.IsValid)
            {
                _requestCinema.Create(reqCinema);

                TempData["success"] = "your request has been added successfully ";
                return RedirectToAction("Index");
            }
            return View(reqCinema); 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Accept(int id)
        {

            var cinema = _requestCinema.GetOne(e => e.Id == id);
            if (cinema != null)
            {
                _cinemaRepository.Create(new()
                {
                    Name = cinema.Name,
                    Description = cinema.Description,
                    CinemaLogo=cinema.CinemaLogo,
                    Address = cinema.Address

                });


                _requestCinema.Delete(cinema);
                _requestCinema.Saving();

                TempData["success"] = "Done";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
        [Authorize(Roles = "Admin ")]
        public IActionResult Delete(int id)
        {

            var cinema = _requestCinema.GetOne(e => e.Id == id);
            if (cinema != null)
            {
                _requestCinema.Delete(cinema);
                _requestCinema.Saving();

                TempData["success"] = "Delete cinema successfuly";

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }
    }
}
