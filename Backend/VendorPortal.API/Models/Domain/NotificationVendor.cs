namespace VendorPortal.API.Models.Domain
{
    public class NotificationVendor
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public string Route { get; set; } // Add this property to store the route

        public DateTime CreatedAt { get; set; }
        // Add any other fields as required
    }
}
