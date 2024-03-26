using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO.ProjectDto
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProjectHeadId { get; set; }

        [Required]
        public string ProjectStatus { get; set; }

        public string? Description { get; set; }
    }
}
