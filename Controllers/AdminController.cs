using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListingApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Panel()
        {
            if (!EnsureAdmin())
                return RedirectToAction("Auth", "Account");

            var model = new AdminDashboardViewModel
            {
                Users = await _context.Users.ToListAsync(),

                UsersCount = await _context.Users.CountAsync(),

                ActiveUsersCount = await _context.Users
                    .CountAsync(x => x.IsActive),

                BlockedUsersCount = await _context.Users
                    .CountAsync(x => !x.IsActive),

                ListingsCount = await _context.Listings.CountAsync(),

                ListingsLast7Days = await _context.Listings
                    .CountAsync(x => x.CreatedAt >= DateTime.Now.AddDays(-7)),

                ListingsLast30Days = await _context.Listings
                    .CountAsync(x => x.CreatedAt >= DateTime.Now.AddDays(-30))
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(int id)
        {
            if (!EnsureAdmin())
                return RedirectToAction("Auth", "Account");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(int id)
        {
            if (!EnsureAdmin())
                return RedirectToAction("Auth", "Account");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.IsActive = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Panel");
        }
    }
}
