using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO
{
    public class VendorDto
    {
        public string OrganizationName { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        [MaxLength(6,ErrorMessage ="Pincode must be of 6 digits")]
        public int Pincode { get; set; }

        [Required]
        public Guid VendorCategoryId { get; set; }

    }
}
