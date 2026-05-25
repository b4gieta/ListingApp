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
        private readonly IWebHostEnvironment _environment;

        public ListingController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        private List<SelectListItem> GetCategories()
        {
            return _db.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            var vm = new ListingFormViewModel
            {
                Categories = GetCategories()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(ListingFormViewModel vm)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            if (!ModelState.IsValid)
            {
                vm.Categories = GetCategories();
                return View(vm);
            }

            int userId = int.Parse(userIdString);

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return RedirectToAction("Auth", "Account");

            string? fileName = null;

            if (vm.Image != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string ext = Path.GetExtension(vm.Image.FileName);
                fileName = $"{Guid.NewGuid()}{ext}";
                string path = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                vm.Image.CopyTo(stream);
            }

            var listing = new Listing
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                Location = vm.Location,
                CategoryId = vm.CategoryId,
                UserId = user.UserId,
                ImageFileName = fileName
            };

            _db.Listings.Add(listing);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);

            if (listing.UserId.ToString() != userIdString && userRole != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            _db.Listings.Remove(listing);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);
            if (listing == null) return RedirectToAction("Index", "Home");

            if (listing.UserId.ToString() != userIdString) return RedirectToAction("Index", "Home");

            var vm = new ListingFormViewModel
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId,
                Categories = GetCategories(),
                ExistingImage = listing.ImageFileName
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, ListingFormViewModel vm)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            var listing = _db.Listings.FirstOrDefault(a => a.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");

            if (listing.UserId.ToString() != userIdString) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                vm.Categories = GetCategories();
                return View(vm);
            }

            listing.Title = vm.Title;
            listing.Description = vm.Description;
            listing.Price = vm.Price;
            listing.Location = vm.Location;
            listing.CategoryId = vm.CategoryId;
            listing.LastEditedAt = DateTime.UtcNow;

            if (vm.Image != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(listing.ImageFileName))
                {
                    string oldPath = Path.Combine(uploadsFolder, listing.ImageFileName);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.Image.FileName)}";
                string path = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                vm.Image.CopyTo(stream);

                listing.ImageFileName = fileName;
            }

            _db.SaveChanges();

            return RedirectToAction("Details", new { id = listing.Id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var listing = _db.Listings
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");

            var vm = new ListingDetailsViewModel
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CreatedAt = listing.CreatedAt,
                LastEditedAt = listing.LastEditedAt,
                CategoryName = listing.Category.Name,
                Username = listing.User.Login,
                ImageFileName = listing.ImageFileName
            };

            return View(vm);
        }
    }
}