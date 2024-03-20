namespace VendorPortal.API.Models.Domain
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime SendOn { get; set; }
        public int Amount { get; set; }
        public string GRNNo { get; set; }
        public Guid GRNId { get; set; }
        public string OrderNo { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public bool IsAccepted { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime DueDate { get;set; }
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public GRN GRN { get; set; }
 
    }
}
