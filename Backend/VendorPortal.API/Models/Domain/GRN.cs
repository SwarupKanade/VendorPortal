namespace VendorPortal.API.Models.Domain
{
    public class GRN
    {
        public Guid Id { get; set; }
        public Guid? PreviousRevisionId { get; set; }
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

        // Navigation Property
        public PurchaseOrder PurchaseOrder { get; set; }

        public GRNHistory replicate()
        {
            var grnHistory = new GRNHistory
            {
                PreviousRevisionId = this.PreviousRevisionId,
                GRNNo = this.GRNNo,
                PurchaseOrderId = this.PurchaseOrderId,
                SendOn = this.SendOn,
                IsAccepted = this.IsAccepted,
                AcceptedOn = this.AcceptedOn,
                ShipmentStatus = this.ShipmentStatus,
                InvoiceStatus = this.InvoiceStatus,
                DocumentPath = this.DocumentPath,
                Comment = this.Comment,
                CreatedOn = this.CreatedOn,
                LastModifiedOn = this.LastModifiedOn,
            };

            return grnHistory;
        }
    }
}