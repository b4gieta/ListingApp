namespace ListingApp.Models
{
    public class ListingImage
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;
    }
}