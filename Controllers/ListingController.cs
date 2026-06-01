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

            var listing = new Listing
            {
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                Location = vm.Location,
                CategoryId = vm.CategoryId,
                UserId = user.UserId
            };

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            foreach (var image in vm.NewImages)
            {
                string ext = Path.GetExtension(image.FileName);
                string fileName = $"{Guid.NewGuid()}{ext}";
                string path = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                image.CopyTo(stream);

                listing.Images.Add(new ListingImage
                {
                    FileName = fileName
                });
            }

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

            var listing = _db.Listings
                .Include(l => l.Images)
                .FirstOrDefault(l => l.Id == id);
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

                ExistingImages = listing.Images
                   .Select(i => new ListingImageViewModel
                   {
                       Id = i.Id,
                       FileName = i.FileName
                   })
                   .ToList()
                    };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, ListingFormViewModel vm)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Auth", "Account");

            var listing = _db.Listings
                .Include(l => l.Images)
                .FirstOrDefault(l => l.Id == id);

            if (listing == null) return RedirectToAction("Index", "Home");
            if (listing.UserId.ToString() != userIdString) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                vm.Categories = GetCategories();

                vm.ExistingImages = listing.Images
                    .Select(i => new ListingImageViewModel
                    {
                        Id = i.Id,
                        FileName = i.FileName
                    })
                    .ToList();

                return View(vm);
            }

            listing.Title = vm.Title;
            listing.Description = vm.Description;
            listing.Price = vm.Price;
            listing.Location = vm.Location;
            listing.CategoryId = vm.CategoryId;
            listing.LastEditedAt = DateTime.UtcNow;

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            if (vm.ImagesToDelete.Any())
            {
                var imagesToDelete = listing.Images
                    .Where(i => vm.ImagesToDelete.Contains(i.Id))
                    .ToList();

                foreach (var image in imagesToDelete)
                {
                    string path = Path.Combine(uploadsFolder, image.FileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    _db.ListingImages.Remove(image);
                }
            }

            foreach (var image in vm.NewImages)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                string path = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                image.CopyTo(stream);
                listing.Images.Add(new ListingImage { FileName = fileName });
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
                .Include(a => a.Images)
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
                ExistingImages = listing.Images
                   .Select(i => new ListingImageViewModel
                   {
                       Id = i.Id,
                       FileName = i.FileName
                   })
                   .ToList()

            };

            return View(vm);
        }
    }
}