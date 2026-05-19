using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ListingApp.Data;
using ListingApp.Models;

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

            var users = await _context.Users.ToListAsync();

            return View(users);
        }
    }
}
