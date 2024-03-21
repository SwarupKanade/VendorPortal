namespace VendorPortal.API.Models.DTO.GRNDto
{
    public class GRNUpdateDto
    {
        public string GRNNo { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public bool ShipmentStatus { get; set; }
        public IFormFile? Document { get; set; }
    }
}
