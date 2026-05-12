using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ListingApp.ViewModels
{
    public class CreateListingViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 999999999)]
        public double Price { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
