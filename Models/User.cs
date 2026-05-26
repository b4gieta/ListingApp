using System.ComponentModel.DataAnnotations;

namespace ListingApp.Models
{
    public enum Role
    {
        User,
        Admin
    }
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Password { get;set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(9)]
        public string TelephoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;


        public Role UserRole { get; set; } = Role.User;

        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
