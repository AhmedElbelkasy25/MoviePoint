using Microsoft.AspNetCore.Mvc;

namespace Movie_Point.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
