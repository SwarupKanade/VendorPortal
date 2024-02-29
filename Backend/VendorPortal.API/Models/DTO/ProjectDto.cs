namespace VendorPortal.API.Models.Dto
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public string ProjectHeadId { get; set; }
        public string ProjectHeadName { get; set; }
        public string ProjectStatus { get; set; }
        public string CreatedOn { get; set; }
        public string? Description { get; set; }
    }
}
