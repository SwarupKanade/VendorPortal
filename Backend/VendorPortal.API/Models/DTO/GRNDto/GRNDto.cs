using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO.GRNDto
{
    public class GRNDto
    {
        public string GRNNo { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public bool ShipmentStatus { get; set; }
        public IFormFile Document { get; set; }
        public string Comment { get; set; }
    }
}
