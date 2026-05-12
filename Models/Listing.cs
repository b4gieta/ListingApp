using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListingApp.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastEditedAt { get; set; }

        // Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
