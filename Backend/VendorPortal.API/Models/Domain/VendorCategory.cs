using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.Domain
{
    public class VendorCategory
    {
        [Key]
        public string VendorId { get; set; }
        [Key]
        public string CategoryId { get; set; }

       // public Category Category { get; set; }
    }
}
