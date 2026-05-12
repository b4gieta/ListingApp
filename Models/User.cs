using System.ComponentModel.DataAnnotations;

namespace ListingApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
