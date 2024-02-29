namespace VendorPortal.API.Models.DTO
{
    public class AdminResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Mobile Number")]
        public string PhoneNumber { get; set; }

    }
}
