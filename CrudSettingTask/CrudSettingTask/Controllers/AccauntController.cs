using CrudSettingTask.Models;
using CrudSettingTask.ViewModels;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using CrudSettingTask.Services.Interface;

namespace CrudSettingTask.Controllers
{
    public class AccauntController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        public AccauntController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IFileService fileService,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _emailService = emailService;
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(ConfirmEmail), "Accaunt", new { userId = user.Id, token },
                    Request.Scheme, Request.Host.ToString());

                var body = await _fileService.ReadFileAsync("wwwroot/templates/verify.html");
                body = body.Replace("{{link}}", link);
                var subject = "Email Confirm";
                
                _emailService.Send(user.Email, subject, body);

                return RedirectToAction("EmailAlert");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
       
 
        public IActionResult EmailAlert()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("İstifadəçi tapılmadı.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Accaunt");
            }

            return View();
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if(forgetPasswordVM == null) return BadRequest();   
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(ResetPassword), "Accaunt", new { userId = user.Id, token },
                Request.Scheme, Request.Host.ToString());

            var body = await _fileService.ReadFileAsync("wwwroot/templates/verify.html");
            body = body.Replace("{{link}}", link);
            var subject = "Reset Password";

            _emailService.Send(user.Email, subject, body);

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId,string token)
        {
            ResetPasswordVM resetPassword = new ResetPasswordVM()
            {
                UserId = userId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (resetPasswordVM == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user == null) return NotFound();

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(resetPasswordVM); 
            }

            return RedirectToAction("Login");
        }


    }
}
