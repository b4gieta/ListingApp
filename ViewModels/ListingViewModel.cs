using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ListingApp.ViewModels
{
    public class ListingViewModel
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

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
