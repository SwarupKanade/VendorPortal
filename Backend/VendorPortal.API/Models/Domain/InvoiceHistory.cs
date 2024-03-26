namespace VendorPortal.API.Models.Domain
{
    public class InvoiceHistory
    {
        public Guid Id { get; set; }
        public Guid? PreviousRevisionId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime SendOn { get; set; }
        public int Amount { get; set; }
        public Guid GRNId { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public bool PaymentStatus { get; set; } = false;
        public DateTime DueDate { get;set; }
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
 
    }
}
