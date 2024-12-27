using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Movie_Point.Models;
using Movie_Point.Models.ViewModels;

namespace Movie_Point.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public UserController(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> rolemanage)
        {
            _userManager = userManager;
            _rolemanager = rolemanage;
        }
        public IActionResult Index(string? account)
        {
            if (account==null)
            {
                return View(_userManager.Users.ToList());
            }

            var Users = _userManager.Users.Where(e =>
                 e.Email.Contains(account) || e.UserName.Contains(account)).ToList();
            if (Users.Any())
            {

                return View(model: Users);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchForUser(string account)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { account = account });

            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> blockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                var result =await _userManager.SetLockoutEndDateAsync(user,DateTime.Now.AddYears(100));
                //await _userManager.SetLockoutEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    user.ISBlocked = true;
                    await _userManager.UpdateAsync(user);
                    TempData["success"] = "The user has been blocked succefully";
                    var allUser = _userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "The user has not been blocked";
                var allUser2 = _userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("NotFoundPage");
        }

        public async Task<IActionResult> unBlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //await _userManager.SetLockoutEnabledAsync(user, true);
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                await _userManager.SetLockoutEnabledAsync(user, false);
                
                if (result.Succeeded)
                {
                    user.ISBlocked = false;
                    await _userManager.UpdateAsync(user);
                    TempData["success"] = "The user is not blocked from thd site";
                    var allUser = _userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "something error has happened";
                
                var allUser2 = _userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("NotFoundPage","home");
        }
        [HttpGet]
        public async Task<IActionResult> AddToRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _userManager.GetRolesAsync(user);
            var roles = _rolemanager.Roles.ToList();
            return View(new AddUserToRoleVM()
            {   
                Id = user.Id,
                Name = user.Name,
                Role = string.Join("", role),
                Roles = roles
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleVM userfrmVM)
        {
            
            var user = await _userManager.FindByIdAsync(userfrmVM.Id);
            if (user != null)
            {
                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);
                var result = await _userManager.AddToRoleAsync(user, userfrmVM.Role);
                if (result.Succeeded)
                {
                    TempData["success"] = "The User Role Has Been Changed";
                }
                return View("index", _userManager.Users.ToList() );
            }
            return RedirectToAction("NotFoundPage" , "home");
        }
    }
}
