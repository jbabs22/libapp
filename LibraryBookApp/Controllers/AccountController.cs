using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryBookApp.Models;
using LibraryBookApp.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace LibraryBookApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        // LOGIN TO APP
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Attempting to log in user with email: {Email}", model.Email);
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in successfully with email: {Email}", model.Email);
                    return RedirectToAction("Index", "Home");
                }
                _logger.LogWarning("Invalid login attempt for email: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            else
            {
                // Log model state errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("Model state error: {ErrorMessage}", error.ErrorMessage);
                }
            }
            return View(model);
        }

        // LOGOUT OF APP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logging out.");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        // REGISTER USER IN APP
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
                _logger.LogInformation("Creating a new user with email: {Email}", model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully. Signing in user with email: {Email}", model.Email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Error creating user: {Error}", error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);

        }

    }
}