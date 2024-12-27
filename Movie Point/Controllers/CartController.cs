using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Point.Models;
using Movie_Point.Models.ViewModels;
using Movie_Point.Repository.IRepository;
using Stripe.Checkout;
using Stripe;

namespace Movie_Point.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Cart> _cartRepository;

        public CartController( UserManager<ApplicationUser> userManager , ICartRepository cart)
        {
            this._userManager = userManager;
            this._cartRepository = cart;
        }
        public IActionResult Index()
        {
            var carts = _cartRepository.Get(includeProps: e => e.Include(e => e.Movie),
                filter: e => e.ApplicationUserId == _userManager.GetUserId(User)).ToList();
            var total = carts.Sum(e=> e.Movie.Price * e.Count);

            return View(model:new CartWithCountVM() {
                Carts = carts,
                Total = total

            });
        }

        public IActionResult AddToCart(int MovieId , int Count=1)
        {
            var userId = _userManager.GetUserId(User);
            var cartInDB = _cartRepository.GetOne(filter: e => e.MovieId == MovieId);
            if (cartInDB != null)
            {
                cartInDB.Count++;
                _cartRepository.Saving();
            }
            else
            {
                _cartRepository.Create(new()
                {
                    MovieId = MovieId,
                    ApplicationUserId = userId,
                    Count = Count,
                    Time = DateTime.Now
                }); 
            }
            TempData["success"] = "the Ticket has been added successfully";
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Increament(int MovieId)
        {
            var movie = _cartRepository.GetOne(filter:e =>e.MovieId == MovieId);
            movie.Count++;
            _cartRepository.Saving();
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult decreament(int MovieId)
        {
            var movie = _cartRepository.GetOne(filter: e => e.MovieId == MovieId);
            if (movie.Count > 1)
            {
                movie.Count--;
                _cartRepository.Saving();
            }

            return RedirectToAction("Index", "Cart");
        }
        public ActionResult deleteTicket(int MovieId)
        {
            var movie = _cartRepository.GetOne(filter: e => e.MovieId == MovieId);
            _cartRepository.Delete(movie);

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Pay()
        {
            
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>()
            ,
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };
            var carts = _cartRepository.Get(includeProps: e => e.Include(e => e.Movie),
                filter: e => e.ApplicationUserId == _userManager.GetUserId(User)).ToList();
            foreach (var item in carts)
            {
                options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Movie.Name,
                                Description = item.Movie.Description,
                            },
                            UnitAmount = (long)item.Movie.Price * 100,
                        },
                        Quantity = item.Count,
                    });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }

    }
}
