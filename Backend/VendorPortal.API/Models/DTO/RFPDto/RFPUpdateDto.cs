using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO.RFPDto
{
    public class RFPUpdateDto
    {
        public IFormFile Document { get; set; }
        public DateTime EndDate { get; set; }
    }
}
