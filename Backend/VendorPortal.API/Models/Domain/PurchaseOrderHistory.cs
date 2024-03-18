namespace VendorPortal.API.Models.Domain
{
    public class PurchaseOrderHistory
    {
        public Guid Id { get; set; }
        public Guid? PreviousRevisionId { get; set; }
        public int OrderNo { get; set; }
        public string VendorId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public string DocumentPath { get; set; }
        public int OrderAmount { get; set; }
        public string? TotalGRN { get; set; }
        public string? Invoice { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string Comment { get; set; }
    }
}
