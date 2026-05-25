using ListingApp.Data;
using ListingApp.Models;
using ListingApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ListingApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? query,
            int? categoryId,
            double? minPrice,
            double? maxPrice,
            string? sort)
        {
            IQueryable<Listing> listingsQuery = _context.Listings
                .Include(x => x.Category)
                .Include(x => x.User);

            if (!string.IsNullOrWhiteSpace(query)) listingsQuery = listingsQuery.Where(x => x.Title.ToLower().Contains(query.ToLower()));

            if (categoryId.HasValue) listingsQuery = listingsQuery.Where(x => x.CategoryId == categoryId.Value);
            if (minPrice.HasValue) listingsQuery = listingsQuery.Where(x => x.Price >= minPrice.Value);
            if(maxPrice.HasValue) listingsQuery = listingsQuery.Where(x => x.Price <= maxPrice.Value);

            if (sort == "date_asc") listingsQuery = listingsQuery.OrderBy(x => x.CreatedAt);
            else if (sort == "price_asc") listingsQuery = listingsQuery.OrderBy(x => x.Price);
            else if (sort == "price_desc") listingsQuery = listingsQuery.OrderByDescending(x => x.Price);
            else listingsQuery = listingsQuery.OrderByDescending(x => x.CreatedAt);

            List<ListingListItemViewModel> listings = await listingsQuery
                .Select(x => new ListingListItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Location = x.Location,
                    CategoryName = x.Category.Name,
                    Username = x.User.Login,
                    CreatedAt = x.CreatedAt,
                    ImageFileName = x.ImageFileName
                })
                .ToListAsync();

            ViewBag.Query = query;
            ViewBag.CategoryId = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.Sort = sort;

            ViewBag.Categories = await _context.Categories
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return View(listings);
        }
    }
}