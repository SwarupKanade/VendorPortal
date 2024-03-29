﻿using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO.PurchaseOrderResponseDto
{
    public class PurchaseOrderResponseDto
    {
        public Guid Id { get; set; }
        public int OrderNo { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public string DocumentPath { get; set; }
        public int OrderAmount { get; set; }
        public int TotalGRN { get; set; }
        public bool InvoiceStatus { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Comment { get; set; }
        public Project Project { get; set; }
    }
}
