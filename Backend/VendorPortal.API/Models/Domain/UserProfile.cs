using Microsoft.AspNetCore.Identity;

namespace VendorPortal.API.Models.Domain
{
    public class UserProfile : IdentityUser
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }

    }
}
