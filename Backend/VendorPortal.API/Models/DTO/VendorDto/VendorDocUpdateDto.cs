namespace VendorPortal.API.Models.DTO.VendorDto
{
    public class VendorDocUpdateDto
    {
        public Guid Id { get; set; }
        public IFormFile Document { get; set; }

    }
}
