namespace VendorPortal.API.Models.Dto
{
    public class VendorUpdateDto
    {
        public string Organization { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Pincode { get; set; }
        public string Category { get; set; }
    }
}
