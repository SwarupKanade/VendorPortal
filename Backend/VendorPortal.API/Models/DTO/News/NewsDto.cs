namespace VendorPortal.API.Models.DTO.News
{
    public class NewsDto
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
