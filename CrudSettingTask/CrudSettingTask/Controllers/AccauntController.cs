using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrudSettingTask.Controllers
{
    public class AccauntController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccauntController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.UserName,

            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (model == null) return BadRequest();
            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "İstifadəçi tapılmadı.");
                    return View(model);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) 
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");   
        }
    }
}
