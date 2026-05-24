using System.ComponentModel.DataAnnotations;

namespace ListingApp.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(50, ErrorMessage = "Tytuł nie może przekraczać 50 znaków")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis jest wymagany")]
        [MaxLength(5000, ErrorMessage = "Opis nie może przekraczać 5000 znaków")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena jest wymagana")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Lokalizacja jest wymagana")]
        [StringLength(200, ErrorMessage = "Lokalizacja nie może przekraczać 200 znaków")]
        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastEditedAt { get; set; }

        // Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<ListingImage> Images { get; set; } = new();
    }
}
