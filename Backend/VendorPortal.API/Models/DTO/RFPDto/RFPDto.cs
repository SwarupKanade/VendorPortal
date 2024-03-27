using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendorPortal.API.Models.DTO.RFPDto
{
    public class RFPDto
    {
        [Required]
        public string Title { get; set; }

        public IFormFile Document { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid VendorCategoryId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
