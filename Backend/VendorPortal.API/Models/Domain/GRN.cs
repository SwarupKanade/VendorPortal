namespace VendorPortal.API.Models.Domain
{
    public class GRN
    {
        public Guid Id { get; set; }
        public string GRNNo { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public DateTime SendOn { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public bool ShipmentStatus { get; set; }
        public bool InvoiceStatus { get; set; } = false;
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

    }
}