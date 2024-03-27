
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.ProjectDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly VendorPortalDbContext dbContext;
        private readonly UserManager<UserProfile> userManager;

        public ProjectController(VendorPortalDbContext dbContext , UserManager<UserProfile> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ProjectDto projectDto)
        {

            var newProject = new Project
            {
               
                Name = projectDto.Name,
                ProjectHeadId = projectDto.ProjectHeadId,
                ProjectStatus = projectDto.ProjectStatus,
                CreatedOn = DateTime.Now,
                Description = projectDto.Description,
            };

            await dbContext.Projects.AddAsync(newProject);
            await dbContext.SaveChangesAsync();

            // Add notification for the project head
            await AddNotification(projectDto.ProjectHeadId, projectDto.Name, newProject.Id.ToString());

            return Ok(newProject);

        }

        private async Task AddNotification(string projectHeadId, string projectName, string projectId)
        {
            var adminRole = "Admin"; // Assuming the role name for admin is "Admin"

            // Find the admin users with the specified role
            var adminUsers = await userManager.GetUsersInRoleAsync(adminRole);

            // Iterate through each admin user and send them the notification
            foreach (var adminUser in adminUsers)
            {
                if (!string.IsNullOrEmpty(projectHeadId))
                {
                    var projectHeadNotification = new NotificationProjectHead
                    {
                        ProjectHeadId = projectHeadId,
                        Content = $"You have been added to the new project: {projectName}",
                        Route = "/assigned-project", // Set the appropriate route for project head notification
                        CreatedAt = DateTime.Now
                    };

                    await dbContext.NotificationsProjectHead.AddAsync(projectHeadNotification);
                }

                var adminNotification = new NotificationAdmin
                {
                    AdminId = adminUser.Id,
                    Content = $"Project Head {projectHeadId} is added to {projectName}",
                    Route = "/projectHead-list", // Set the appropriate route for admin notification
                    CreatedAt = DateTime.Now
                };

                await dbContext.AdminNotifications.AddAsync(adminNotification);
            }

            await dbContext.SaveChangesAsync();
        }



        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var projectResult = await dbContext.Projects.Include("UserProfile").FirstOrDefaultAsync(x => x.Id == id);

            if (projectResult != null)
            {
                var project = new ProjectResponseDto
                {
                    Id = projectResult.Id,
                    Name = projectResult.Name,
                    ProjectStatus = projectResult.ProjectStatus,
                    CreatedOn = projectResult.CreatedOn,
                    Description = projectResult.Description,
                    ProjectHeadId = projectResult.UserProfile.Id,
                    ProjectHeadName = projectResult.UserProfile.Name,
                };

                return Ok(project);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProjectUpdateDto projectUpdateDto)
        {
            var projectResult = await dbContext.Projects.Include("UserProfile").FirstOrDefaultAsync(x => x.Id == id);

            if (projectResult != null)
            {
                projectResult.Name = projectUpdateDto.Name;
                projectResult.ProjectStatus = projectUpdateDto.ProjectStatus;
                projectResult.Description = projectUpdateDto.Description;

                await dbContext.SaveChangesAsync();

                var project = new ProjectResponseDto
                {
                    Id = projectResult.Id,
                    Name = projectResult.Name,
                    ProjectStatus = projectResult.ProjectStatus,
                    CreatedOn = projectResult.CreatedOn,
                    Description = projectResult.Description,
                    ProjectHeadId = projectResult.UserProfile.Id,
                    ProjectHeadName = projectResult.UserProfile.Name,
                };
                return Ok(project);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery]string? filterVal)
        {
            var projectsResult = dbContext.Projects.Include("UserProfile").AsQueryable();
            

            if(String.IsNullOrWhiteSpace(filterOn)==false && String.IsNullOrWhiteSpace(filterVal) == false)
            {
                if (filterOn.Equals("projectHead",StringComparison.OrdinalIgnoreCase))
                {
                    projectsResult = projectsResult.Where(x=>x.UserProfile.Name.ToLower().Contains(filterVal.ToLower()));
                    
                }
                if(filterOn.Equals("projectStatus", StringComparison.OrdinalIgnoreCase))
                {
                    projectsResult = projectsResult.Where(x=>x.ProjectStatus.ToLower().Contains(filterVal.ToLower()));
                }
            }

            if (projectsResult != null)
            {
                List<ProjectResponseDto> result = new List<ProjectResponseDto>();
                foreach (var project in projectsResult)
                {
                    var newProject = new ProjectResponseDto
                    {
                        Id = project.Id,
                        Name = project.Name,
                        ProjectStatus = project.ProjectStatus,
                        CreatedOn = project.CreatedOn,
                        Description = project.Description,
                        ProjectHeadId = project.UserProfile.Id,
                        ProjectHeadName = project.UserProfile.Name,
                    };
                    result.Add(newProject);
                }
                
                return Ok(result);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("ProjectHead/{id:Guid}")]
        public async Task<IActionResult> GetAssignedProjectByUserId([FromRoute] string id)
        {

            var projectsResult = await  dbContext.Projects.Include(u => u.UserProfile).Where(x => x.ProjectHeadId == id).ToListAsync();


            if (projectsResult != null)
            {
                List<ProjectResponseDto> allProject = new List<ProjectResponseDto>();
                foreach(var project in projectsResult)
                {
                    var newProject = new ProjectResponseDto
                    {
                        Id = project.Id,
                        Name = project.Name,
                        ProjectStatus = project.ProjectStatus,
                        CreatedOn = project.CreatedOn,
                        Description = project.Description,
                        ProjectHeadId = project.UserProfile.Id,
                        ProjectHeadName = project.UserProfile.Name,
                    };
                    allProject.Add(newProject);
                }
                

                return Ok(allProject);
            }

            return BadRequest("Something went wrong");
        }



    }
}
