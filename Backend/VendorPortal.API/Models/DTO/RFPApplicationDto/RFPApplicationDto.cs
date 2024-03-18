namespace VendorPortal.API.Models.DTO.RFPApplicationDto
{
    public class RFPApplicationDto
    {
        public Guid RFPId { get; set; }
        public string VendorId { get; set; }
        public IFormFile Document { get; set; }
        public string Comment { get; set; }
    }
}
