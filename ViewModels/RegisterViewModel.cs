using System.ComponentModel.DataAnnotations;

namespace ListingApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Login musi mieć od 3 do 50 znaków")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [RegularExpression(@"^\d{9}$",
            ErrorMessage = "Numer telefonu musi zawierać 9 cyfr")]
        public string TelephoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lokalizacja jest wymagana")]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Hasło musi mieć minimum 8 znaków")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Hasło musi zawierać: dużą literę, małą literę, cyfrę oraz znak specjalny")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
