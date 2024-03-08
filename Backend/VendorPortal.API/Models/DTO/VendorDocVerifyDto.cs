namespace VendorPortal.API.Models.DTO
{
    public class VendorDocVerifyDto
    {
        public string VendorId { get; set; }
        public Guid DocumentId { get; set; }
        public bool DocumentVerified { get; set; }
        public string? Comment { get; set; }

    }
}
