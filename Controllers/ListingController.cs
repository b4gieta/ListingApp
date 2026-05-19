using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ListingApp.Controllers
{
    public class ListingController : Controller
    {
        private readonly AppDbContext _db;

        public ListingController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new ListingViewModel
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
        public IActionResult Create(ListingViewModel vm)
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
                    Username = "Test",
                    Email = "test@test.com"
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
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            _db.Listings.Add(listing);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);

            if (listing != null)
            {
                _db.Listings.Remove(listing);
                _db.SaveChanges();
            }           

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);

            if (listing == null) return NotFound();

            var vm = new ListingViewModel
            {
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId,

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
        public IActionResult Edit(int id, ListingViewModel vm)
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

            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");

            listing.Title = vm.Title;
            listing.Description = vm.Description;
            listing.Price = vm.Price;
            listing.Location = vm.Location;
            listing.CategoryId = vm.CategoryId;

            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var listing = _db.Listings
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");

            var vm = new ListingViewModel
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId
            };

            return View(vm);
        }
    }
}