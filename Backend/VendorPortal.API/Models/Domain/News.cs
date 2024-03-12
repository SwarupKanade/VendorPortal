namespace VendorPortal.API.Models.Domain
{
    public class News
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}
