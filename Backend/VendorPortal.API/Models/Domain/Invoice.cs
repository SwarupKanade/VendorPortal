namespace VendorPortal.API.Models.Domain
{
    public class Invoice
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
        public DateTime DueDate { get; set; }
        public string DocumentPath { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        // Navigation Property
        public GRN GRN { get; set; }

        public InvoiceHistory replicate()
        {
            var invoiceHistory = new InvoiceHistory
            {
                PreviousRevisionId = this.PreviousRevisionId,
                InvoiceNo = this.InvoiceNo,
                SendOn = this.SendOn,
                Amount = this.Amount,
                GRNId = this.GRNId,
                IsAccepted = this.IsAccepted,
                AcceptedOn = this.AcceptedOn,
                PaymentStatus = this.PaymentStatus,
                DueDate = this.DueDate,
                DocumentPath = this.DocumentPath,
                Comment = this.Comment,
                CreatedOn = this.CreatedOn,
                LastModifiedOn = this.LastModifiedOn,
            };
            return invoiceHistory;
        }
    }
}
