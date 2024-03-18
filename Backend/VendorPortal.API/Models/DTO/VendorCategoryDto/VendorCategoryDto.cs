using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO.VendorCategoryDto
{
    public class VendorCategoryDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public List<Guid> DocumentList { get; set; }
    }
}
