using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie_Point.Models;
using Movie_Point.Repository.IRepository;
using Movie_Point.Utility;

namespace Movie_Point.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IRepository<Cart> _cartrepository;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IEmailSender _emailSender;

        public CheckoutController(ICartRepository cartrepository , UserManager<ApplicationUser> usermanager , IEmailSender emailSender)
        {
            this._cartrepository = cartrepository;
            this._usermanager = usermanager;
            this._emailSender = emailSender;
        }
        public async Task<IActionResult> success()
        {
            //var userId = _usermanager.GetUserId(User);
            var user = await _usermanager.GetUserAsync(User);
            await _emailSender.SendEmailAsync(user.Email,"Successful Pay",

               "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    \r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f8f9fa;\r\n            display: flex;\r\n            justify-content: center;\r\n            align-items: center;\r\n            height: 100vh;\r\n            color: #333;\r\n        }\r\n        .container {\r\n            text-align: center;\r\n            background: white;\r\n            padding: 30px;\r\n            border-radius: 10px;\r\n            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .container h1 {\r\n            color: #28a745;\r\n            font-size: 2rem;\r\n            margin-bottom: 10px;\r\n        }\r\n        .container p {\r\n            margin: 10px 0;\r\n            font-size: 1.2rem;\r\n        }\r\n        .container img {\r\n            margin: 20px 0;\r\n            max-width: 100%;\r\n            border-radius: 10px;\r\n        }\r\n        .container .btn {\r\n            display: inline-block;\r\n            margin-top: 20px;\r\n            padding: 10px 20px;\r\n            font-size: 1rem;\r\n            color: white;\r\n            background-color: #007bff;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            transition: background-color 0.3s;\r\n        }\r\n        .container .btn:hover {\r\n            background-color: #0056b3;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Payment Successful!</h1>\r\n        <p>Thank you for purchasing your movie tickets.</p>\r\n                       <p>Enjoy your movie!</p>\r\n        <a href=\"www.gmail.com\" class=\"btn\">Return to Home</a>\r\n    </div>\r\n</body>\r\n</html>\r\n"



                );
            
            
            var carts = _cartrepository.Get(filter: e => e.ApplicationUserId == user.Id);
            foreach (var cart in carts)
            {   
                _cartrepository.Delete(cart);
            }
            _cartrepository.Saving();
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
