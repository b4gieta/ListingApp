namespace ListingApp.ViewModels
{
    public class ListingDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastEditedAt { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public string? ImageFileName { get; set; }
    }
}
