namespace VendorPortal.API.Models.DTO.VendorDto
{
    public class VendorDocVerifyDto
    {
        public Guid Id { get; set; }
        public bool DocumentVerified { get; set; }
        public string? Comment { get; set; }

    }
}
