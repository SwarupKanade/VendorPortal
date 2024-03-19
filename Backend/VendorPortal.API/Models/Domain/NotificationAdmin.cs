namespace VendorPortal.API.Models.Domain
{
    public class NotificationAdmin
    {
        public int Id { get; set; }

        public string AdminId { get; set; } // Add this property to store the admin ID

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        // Add any other fields as required

    }
}
