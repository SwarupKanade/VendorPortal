namespace VendorPortal.API.Models.DTO.NewsDto
{
    public class NewsUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
