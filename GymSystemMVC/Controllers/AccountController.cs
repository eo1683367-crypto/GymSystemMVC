using GymSystemMVC.BLL.ViewModels.ApplicationViewModel;
using GymSystemMVC.Controllers;
using GymSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemMVC.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,CancellationToken ct)
        {
           if(!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);

            if(user is null || string.IsNullOrEmpty(user.UserName)) 
            {
                ModelState.AddModelError(string.Empty,"Invalid Email Or Password");
                return View(model);
            }
            var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "This Account Is LockOut");

            if (result.IsNotAllowed)
                ModelState.AddModelError(string.Empty, "Un Authorized");

            else
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
