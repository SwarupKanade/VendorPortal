namespace VendorPortal.API.Models.Domain
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public Guid? PreviousRevisionId { get; set; }
        public int OrderNo { get; set; }
        public string VendorId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public string DocumentPath { get; set; }
        public int OrderAmount { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string Comment { get; set; }

        // Navigation Property
        public UserProfile Vendor { get; set; }
        public Project Project { get; set; }

        public PurchaseOrderHistory replicate()
        {
            var purchaseOrderHistory = new PurchaseOrderHistory
            {
                PreviousRevisionId = this.PreviousRevisionId,
                OrderNo = this.OrderNo,
                VendorId = this.VendorId,
                ProjectId = this.ProjectId,
                ReleaseDate = this.ReleaseDate,
                ExpectedDelivery = this.ExpectedDelivery,
                DocumentPath = this.DocumentPath,
                OrderAmount = this.OrderAmount,
                IsAccepted = this.IsAccepted,
                AcceptedOn = this.AcceptedOn,
                IsActive = this.IsActive,
                CreatedOn = this.CreatedOn,
                LastModifiedOn = this.LastModifiedOn,
                Comment = this.Comment
            };

            return purchaseOrderHistory;
        }


    }
}
