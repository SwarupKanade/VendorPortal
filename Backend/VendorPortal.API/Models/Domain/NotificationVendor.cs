namespace VendorPortal.API.Models.Domain
{
    public class NotificationVendor
    {
        public int Id { get; set; }
        public string UserId { get; set; } // The ID of the user to be notified
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        // Add any other fields as required
    }
}
