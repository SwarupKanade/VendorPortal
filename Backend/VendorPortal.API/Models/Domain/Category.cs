using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.Domain
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string DocumentList { get; set; }

    }
}
