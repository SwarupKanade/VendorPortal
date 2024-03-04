using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VendorPortal.API.Models.Domain;

namespace VendorPortal.API.Models.DTO
{
    public class ProductDto
    {
        public string Name { get; set; }
        public IFormFile FormFile { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string UnitType { get; set; }
        public string? Specification { get; set; }
        public string? ProductCategory { get; set; }
        public string? SubCategory { get; set; }
        public Guid SizeId { get; set; }
    }
}
