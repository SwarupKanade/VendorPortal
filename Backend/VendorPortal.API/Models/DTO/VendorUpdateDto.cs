namespace VendorPortal.API.Models.DTO
{
    public class VendorUpdateDto
    {
        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        [MaxLength(6,ErrorMessage ="Pincode must be of 6 digits")]
        public int Pincode { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Uppercase,Symbol & Numbers combination required")]
        public string CurrentPassword { get; set; } = string.Empty;

        [DataType(DataType.Password, ErrorMessage = "Uppercase,Symbol & Numbers combination required")]
        public string NewPassword { get; set; } = string.Empty;
        
        public List<IFormFile>? Documents { get; set; }

    }
}
