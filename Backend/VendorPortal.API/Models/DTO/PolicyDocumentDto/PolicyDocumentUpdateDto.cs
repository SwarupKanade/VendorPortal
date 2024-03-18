namespace VendorPortal.API.Models.DTO.PolicyDocumentDto
{
    public class PolicyDocumentUpdateDto
    {
        public string Name { get; set; }
        public IFormFile? Document { get; set; }
        public bool IsActive { get; set; }
    }
}
