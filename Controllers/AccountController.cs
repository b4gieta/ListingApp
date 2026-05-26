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

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Konto zostało zablokowane");
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

                FirstName = model.FirstName,
                LastName = model.LastName,
                TelephoneNumber = model.TelephoneNumber,
                Location = model.Location,

                IsActive = true,
                UserRole = Role.User
            };

            user.Password = _passwordHasher.HashPassword(user, model.Password);

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


        // User Profile
        [HttpGet] // Wyświetlanie
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                return NotFound();

            var model = new UserProfileViewModel
            {
                UserId = user.UserId,
                Login = user.Login,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TelephoneNumber = user.TelephoneNumber,
                Location = user.Location
            };

            return View(model);
        }

        [HttpPost] // Edytowanie
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == model.UserId);

            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.TelephoneNumber = model.TelephoneNumber;
            user.Location = model.Location;
            user.Email = model.Email;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
