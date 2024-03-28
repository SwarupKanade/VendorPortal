using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO.PurchaseOrderVendorResponseDto
{
    public class PurchaseOrderVendorResponseDto
    {
        public Guid Id { get; set; }
        public int OrderNo { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string OrganizationName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public int OrderAmount { get; set; }
        public bool? IsAccepted { get; set; }
        public Project Project { get; set; }
        public string Comment { get; set; }

    }
}
