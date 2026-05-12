using System.ComponentModel.DataAnnotations;

namespace ListingApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
