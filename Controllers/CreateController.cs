using ListingApp.Data;
using ListingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ListingApp.ViewModels;

namespace ListingApp.Controllers
{
    public class CreateController : Controller
    {
        private readonly AppDbContext _db;

        public CreateController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new CreateListingViewModel
            {
                Categories = _db.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(CreateListingViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _db.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(vm);
            }

            var user = _db.Users.FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Login = "Test",
                    Email = "test@test.com",
                    Password = "TEMP",
                    UserRole = Role.Admin
                };

                _db.Users.Add(user);

                _db.SaveChanges();
            }

            var listing = new Listing
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                Location = vm.Location,
                CategoryId = vm.CategoryId,
                UserId = user.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Listings.Add(listing);

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}