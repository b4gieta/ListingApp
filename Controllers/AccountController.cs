using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;

namespace ListingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Auth");

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Login == model.Login);

            if (user == null)
            {
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło");
                return View("Auth");
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                model.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło");
                return View("Auth");
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Login", user.Login);
            HttpContext.Session.SetString("UserRole", user.UserRole.ToString());

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Auth");

            bool loginExists = await _context.Users
                .AnyAsync(x => x.Login == model.Login);

            if (loginExists)
            {
                ModelState.AddModelError("Login", "Taki login już istnieje");
                return View("Auth");
            }

            var user = new User
            {
                Login = model.Login,
                Email = model.Email,

                UserRole = Role.User
            };

            user.Password = _passwordHasher.HashPassword(
                user,
                model.Password
            );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Login", user.Login);
            HttpContext.Session.SetString("UserRole", user.UserRole.ToString());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
