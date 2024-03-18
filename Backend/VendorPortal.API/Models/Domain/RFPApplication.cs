namespace VendorPortal.API.Models.Domain
{
    public class RFPApplication
    {
        public Guid Id { get; set; }
        public Guid RFPId { get; set; }
        public string VendorId { get; set; }
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        // Navigation Properties
        public RFP RFP { get; set; }
        public UserProfile Vendor { get; set; }
    }
}