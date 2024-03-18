namespace VendorPortal.API.Models.DTO.RFPApplication
{
    public class RFPApplicationDto
    {
        public Guid RFPId { get; set; }
        public string VendorId { get; set; }
        public IFormFile Document { get; set; }
        public string Comment { get; set; }
    }
}
