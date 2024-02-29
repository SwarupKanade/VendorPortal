using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.API.Models.Dto
{
    public class AdminDto
    {
        public string Organization { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Pincode { get; set; }
        public string Category { get; set; }
        public string Password { get; set; }

    }
}
