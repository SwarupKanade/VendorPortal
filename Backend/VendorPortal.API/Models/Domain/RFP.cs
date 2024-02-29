namespace VendorPortal.API.Models.Domain
{
    public class RFP
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DocumentPath { get; set; }
        public string Project { get; set; }
        public DateOnly EndDate { get;set; }
        public string CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
