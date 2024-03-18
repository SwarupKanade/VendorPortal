namespace VendorPortal.API.Models.DTO.PurchaseOrderDto
{
    public class PurchaseOrderDto
    {
        public int OrderNo { get; set; }
        public string VendorId { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public IFormFile Document { get; set; }
        public int OrderAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
