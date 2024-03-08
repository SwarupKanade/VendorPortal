namespace VendorPortal.API.Models.DTO
{
    public class VendorDocUpdateDto
    {
        public IFormFile Document { get; set; }

        public string VendorId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
