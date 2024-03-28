using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO.AdminDto
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? OrganizationName { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public int? Pincode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
