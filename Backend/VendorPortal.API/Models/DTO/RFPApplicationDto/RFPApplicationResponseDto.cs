using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO.RFPApplication
{
    public class RFPApplicationResponseDto
    {
        public Guid Id { get; set; }
        public Guid RFPId { get; set; }
        public string VendorId { get; set; }
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public string VendorName { get; set; }
        public RFP RFP { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
