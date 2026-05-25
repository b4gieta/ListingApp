namespace ListingApp.ViewModels
{
    public class ListingListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public double Price { get; set; }

        public string Location { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string? ImageFileName { get; set; }
    }
}
