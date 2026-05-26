using ListingApp.Models;

namespace ListingApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<User> Users { get; set; } = new();

        public int UsersCount { get; set; }

        public int ActiveUsersCount { get; set; }

        public int BlockedUsersCount { get; set; }

        public int ListingsCount { get; set; }

        public int ListingsLast7Days { get; set; }

        public int ListingsLast30Days { get; set; }
    }
}
