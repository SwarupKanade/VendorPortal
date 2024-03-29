﻿using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO.InvoiceDto
{
    public class InvoiceDto
    {
        public string InvoiceNo { get; set; }
        public int Amount { get; set; }
        public Guid GRNId { get; set; }
        public DateTime DueDate { get; set; }
        public IFormFile Document { get; set; }
    }
}
