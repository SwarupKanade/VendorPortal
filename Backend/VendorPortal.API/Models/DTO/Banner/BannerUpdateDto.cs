namespace VendorPortal.API.Models.DTO.Banner
{
    public class BannerUpdateDto
    {
        public IFormFile? Image { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
