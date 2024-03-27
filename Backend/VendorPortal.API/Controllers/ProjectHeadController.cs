using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VendorPortal.API.Data;
using VendorPortal.API.Mail;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO;
using VendorPortal.API.Models.DTO.ProjectHeadDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectHeadController : ControllerBase
    {

        private readonly UserManager<UserProfile> userManager;
        private readonly EmailService emailService;
        private readonly VendorPortalDbContext dbContext;

        public ProjectHeadController(UserManager<UserProfile> userManager, EmailService emailService , VendorPortalDbContext dbContext)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.dbContext = dbContext;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] ProjectHeadDto projectHeadDto)
        {

            var newProjectHead = new UserProfile
            {
               
                Name = projectHeadDto.Name,
                PhoneNumber = projectHeadDto.PhoneNumber,
                Email = projectHeadDto.Email,
                UserName = projectHeadDto.UserName,
                IsVerified = true,
            };

            var projectHeadResult = await userManager.CreateAsync(newProjectHead, "Pass@123");

            if (projectHeadResult.Succeeded)
            {
                List<string> roles = new List<string>();
                roles.Add("ProjectHead");
                projectHeadResult = await userManager.AddToRolesAsync(newProjectHead, roles);

                if (projectHeadResult.Succeeded)
                {
                    // Send welcome notification to admin with appropriate route
                    await AddAdminNotification(newProjectHead.Name, "/projectHead-list");


                    // Send welcome notification to project head
                    await AddProjectHeadNotification(newProjectHead.Id.ToString(), newProjectHead.Name);

                    SendWelcomeEmail(newProjectHead);
                    return Ok("ProjectHead was registered! Please login.");
                }
            }

            return BadRequest("Something went wrong");
        }

        private async Task AddAdminNotification(string projectHeadName, string route)
        {
            var adminRole = "Admin"; // Assuming the role name for admin is "Admin"

            // Find the admin users with the specified role
            var adminUsers = await userManager.GetUsersInRoleAsync(adminRole);

            // Iterate through each admin user and send them the notification
            foreach (var adminUser in adminUsers)
            {
                var adminNotification = new NotificationAdmin
                {
                    AdminId = adminUser.Id,
                    Content = $"Project head '{projectHeadName}' was registered.",
                    Route = route, // Set the appropriate route for the notification
                    CreatedAt = DateTime.Now
                };

                await dbContext.AdminNotifications.AddAsync(adminNotification);
            }

            await dbContext.SaveChangesAsync();
        }




        private async Task AddProjectHeadNotification(string projectHeadId, string projectHeadName)
        {
            var projectHeadNotification = new NotificationProjectHead
            {
                ProjectHeadId = projectHeadId,
                Content = $"Welcome to SCIQUS Vendor Portal, {projectHeadName}!",
                Route = "/",
                CreatedAt = DateTime.Now
            };

            await dbContext.NotificationsProjectHead.AddAsync(projectHeadNotification);
            await dbContext.SaveChangesAsync();
        }




        [HttpPost]
        [Route("ChangePassword/{Id:Guid}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string Id, [FromBody] ChangePasswordDto passwordDto)
        {
            var head = await userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (passwordDto.CurrentPassword == passwordDto.NewPassword)
            {
                return BadRequest("Old and New Password are same, Please enter new password.");
            }
            else if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
            {
                return BadRequest("New Password and Confirm Password are mis-matching.");
            }
            else
            {
                var passResult = await userManager.ChangePasswordAsync(head, passwordDto.CurrentPassword, passwordDto.NewPassword);
                if (passResult.Succeeded)
                {
                    return Ok("Password Changed");
                }
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var allProjectHeadResult = await userManager.GetUsersInRoleAsync("ProjectHead");
            allProjectHeadResult = allProjectHeadResult.Where(x => x.Id == id).ToList();

            if (allProjectHeadResult.Any())
            { 
                var projectHeadResult = await userManager.FindByIdAsync(id);

                var projectHead = new ProjectHeadResponseDto
                {
                    Id = projectHeadResult.Id,
                    Name = projectHeadResult.Name,
                    Email = projectHeadResult.Email,
                    PhoneNumber = projectHeadResult.PhoneNumber,
                };

                return Ok(projectHead);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ProjectHeadUpdateDto projectHeadUpdateDto)
        {
            var allProjectHeadResult = await userManager.GetUsersInRoleAsync("ProjectHead");
            allProjectHeadResult = allProjectHeadResult.Where(x => x.Id == id).ToList();

            if (allProjectHeadResult.Any())
            {
                var projectHeadResult = await userManager.FindByIdAsync(id);

                if (projectHeadUpdateDto.NewPassword != "")
                {
                    var passResult = await userManager.ChangePasswordAsync(projectHeadResult, projectHeadUpdateDto.CurrentPassword, projectHeadUpdateDto.NewPassword);
                    if (!passResult.Succeeded)
                    {
                        return BadRequest(passResult.Errors);
                    }
                }
                projectHeadResult.Name = projectHeadUpdateDto.Name;
                projectHeadResult.PhoneNumber = projectHeadUpdateDto.PhoneNumber;

              
                await userManager.UpdateAsync(projectHeadResult);

                var projectHead = new ProjectHeadResponseDto
                {
                    Id = projectHeadResult.Id,
                    Name = projectHeadResult.Name,
                    Email = projectHeadResult.Email,
                    PhoneNumber = projectHeadResult.PhoneNumber,
                };

                return Ok(projectHead);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll([FromQuery] string? nameVal)
        {
            var dbProjectHeadsResult = await userManager.GetUsersInRoleAsync("ProjectHead");
            var projectHeadsResult = dbProjectHeadsResult.AsQueryable();

            if (String.IsNullOrWhiteSpace(nameVal) == false)
            {
                projectHeadsResult = projectHeadsResult.Where(x => x.Name.ToLower().Contains(nameVal.ToLower()));
            }

            if (projectHeadsResult != null)
            {
                List<ProjectHeadResponseDto> allProjectHead = new List<ProjectHeadResponseDto>();
                foreach (var projectHead in projectHeadsResult)
                {
                    var newProjectHead = new ProjectHeadResponseDto
                    {
                        Id = projectHead.Id,
                        Name = projectHead.Name,
                        Email = projectHead.Email,
                        PhoneNumber = projectHead.PhoneNumber,
                    };
                    allProjectHead.Add(newProjectHead);
                }

                return Ok(allProjectHead);
            }
            return BadRequest("Something went wrong");
        }

        private void SendWelcomeEmail(UserProfile user)
        {
            string subject = $"Welcome to Our Application Project Head ID {user.Id}";
            string body = $"Dear {user.Name},\n\nWelcome to our application! Your username is: {user.UserName} and your password is: Pass@123\n\nBest regards,\nYour Application Team";

            emailService.SendEmail(user.Email, subject, body);
        }
    }
}
