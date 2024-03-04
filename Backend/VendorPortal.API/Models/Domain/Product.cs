using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace VendorPortal.API.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }
        public string ImagePath { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string UnitType { get; set; }
        public string? Specification { get; set; }
        public string? ProductCategory { get; set; }
        public string? SubCategory { get; set; }

        [ForeignKey("Id")]
        public Guid  SizeId { get; set; }
        public ProductSize ProductSize { get; set; }
    }
}
