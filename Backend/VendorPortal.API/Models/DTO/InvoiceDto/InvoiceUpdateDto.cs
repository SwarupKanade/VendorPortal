namespace VendorPortal.API.Models.DTO.InvoiceDto
{
    public class InvoiceUpdateDto
    {
        public string InvoiceNo { get; set; }
        public int Amount { get; set; }
        public DateTime DueDate { get; set; }
        public IFormFile? Document { get; set; }
    }
}
