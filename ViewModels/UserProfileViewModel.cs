namespace ListingApp.ViewModels
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string TelephoneNumber { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;
    }
}
