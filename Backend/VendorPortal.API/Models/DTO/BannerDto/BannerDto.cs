namespace VendorPortal.API.Models.DTO.BannerDto
{
    public class BannerDto
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
