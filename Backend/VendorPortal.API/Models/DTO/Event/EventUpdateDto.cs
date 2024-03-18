namespace VendorPortal.API.Models.DTO.Event
{
    public class EventUpdateDto
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
