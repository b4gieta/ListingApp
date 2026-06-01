using System.Diagnostics;
using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var listings = _db.Listings
                .Include(a => a.Category)
                .Include(a => a.User)
                .Include(a => a.Images)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ListingListItemViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Price = a.Price,
                    Location = a.Location,
                    CategoryName = a.Category.Name,
                    Username = a.User.Login,
                    CreatedAt = a.CreatedAt,
                    ImageFileName = a.Images.FirstOrDefault() != null ? a.Images.FirstOrDefault()!.FileName : null
                })
                .ToList();

            return View(listings);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
