using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
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

        [HttpGet]
        public IActionResult Create()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

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
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Auth", "Account");

            if (vm.Images.Count > 8)
                ModelState.AddModelError("Images", "Maksymalnie 8 zdjęć.");

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

            int userId = int.Parse(userIdString);

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return RedirectToAction("Auth", "Account");

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

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            foreach (var image in vm.Images)
            {
                if (image.Length <= 0) continue;

                string extension = Path.GetExtension(image.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string path = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(path, FileMode.Create)) image.CopyTo(stream);

                _db.ListingImages.Add(new ListingImage
                {
                    FileName = fileName,
                    ListingId = listing.Id
                });
            }

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
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

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
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

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
                .Include(a => a.Images)
                .FirstOrDefault(a => a.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");

            var vm = new ListingViewModel
            {
                Id = listing.Id,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId,
                CategoryName = listing.Category.Name,
                UserId = listing.UserId,
                Username = listing.User.Login,
                CreatedAt = listing.CreatedAt,
                ImagesModels = listing.Images
            };

            return View(vm);
        }
    }
}