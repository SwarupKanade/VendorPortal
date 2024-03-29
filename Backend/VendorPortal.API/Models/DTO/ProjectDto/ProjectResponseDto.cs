﻿namespace VendorPortal.API.Models.DTO.ProjectDto
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Description { get; set; }

        public string ProjectHeadName { get; set; }
        public string ProjectHeadId { get; set; }
    }
}
