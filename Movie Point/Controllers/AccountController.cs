using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Movie_Point.Models;
using Movie_Point.Models.ViewModels;

namespace Movie_Point.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new("Admin"));
                await _roleManager.CreateAsync(new("Cinema"));
                await _roleManager.CreateAsync(new("Customer"));
            }

            //await _userManager.AddToRoleAsync(user, "Admin");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Register(LoginUserVM userLog , IFormFile? file)
        {
            ModelState.Remove("ImgUrl");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AdminImg", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save in db
                    userLog.ImgUrl = fileName;
                }
                ApplicationUser user = new()
                {
                    UserName = userLog.UserName,
                    Name = userLog.Name,
                    Email = userLog.Email,
                    PhoneNumber = userLog.PhoneNumber,
                    ImgUrl = userLog.ImgUrl

                };



                var result = await _userManager.CreateAsync(user,userLog.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    await _signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Password must be has a Capital and Small letters and numbers");
                }



            }

            return View(userLog);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM log)
        {
            if (ModelState.IsValid) 
            {
                var appUserWithEmail = await _userManager.FindByEmailAsync(log.Account);
                var appUserWithUserName = await _userManager.FindByNameAsync(log.Account);

                if (appUserWithEmail != null || appUserWithUserName != null)
                { 
                    var result = await _userManager.CheckPasswordAsync(appUserWithEmail ?? appUserWithUserName
                        , log.Password);
                    if (result)
                    {
                        await _signInManager.SignInAsync(appUserWithEmail ?? appUserWithUserName, log.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Your account or Password is not correct");
                    }
                
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Your account or Password is not correct");
                }

            }
            return View(log);
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(EmailVM userEmail)
        {
            if (ModelState.IsValid)
            {
                var appUserithEmail = await _userManager.FindByEmailAsync(userEmail.Account);
                var appUserithName = await _userManager.FindByNameAsync(userEmail.Account);
                if(appUserithEmail != null || appUserithName != null)
                {

                    return RedirectToAction("ChangeForgottenPassword", "Account" , new {acc = userEmail.Account });
                }
                else
                {
                    ModelState.AddModelError("Account", "This Account Dosn't Exist");
                }

            }
            
                return View(userEmail);
            
        }
        [HttpGet]
        public IActionResult ChangeForgottenPassword(string acc)
        {
            ViewBag.Email= acc;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeForgottenPassword(ForgetPassordVM newpass)
        {
            if (ModelState.IsValid)
            {
                var appUserithEmail = await _userManager.FindByEmailAsync(newpass.Account);
                var appUserithName = await _userManager.FindByNameAsync(newpass.Account);

                await _userManager.RemovePasswordAsync(appUserithEmail ?? appUserithName);
                await _userManager.AddPasswordAsync(appUserithEmail ?? appUserithName, newpass.Password);
                TempData["success"] = "Password has been changed successfully";
                return RedirectToAction("Index", "Home");
            }
            return View(newpass);
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM newpass)
        {
            if (ModelState.IsValid)
            {
                var  appUser = await _userManager.FindByIdAsync(_userManager.GetUserId(User) );

                var result = await _userManager.ChangePasswordAsync(appUser,newpass.OldPassword,newpass.NewPassword);
                if (result.Succeeded)
                {
                    TempData["success"] = "The password has changed correctlly";
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "there is some thing wrong ");
            }
            return View(newpass);
            

        }

        public async Task <IActionResult> GetProfile(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return View(model:user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(model:user);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user, IFormFile? file)
        {
            ModelState.Remove("ImgUrl");
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var oldUser = await _userManager.FindByIdAsync(userId);

                
                if (file != null && file.Length > 0)
                {
                    // Genereate name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AdminImg", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    //delete old img
                    if (oldUser.ImgUrl != null)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AdminImg", oldUser.ImgUrl);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // Save in db
                    oldUser.ImgUrl = fileName;
                }
                
                
                oldUser.Address = user.Address;
                oldUser.Name = user.Name;
                oldUser.UserName = user.UserName;
                oldUser.PhoneNumber = user.PhoneNumber;
                oldUser.Email = user.Email;
                await _userManager.UpdateAsync(oldUser);
                TempData["success"] = "Edit User successfully";
                return RedirectToAction("GetProfile" , "Account", new { name = User.Identity.Name });
            }
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


    }
}
