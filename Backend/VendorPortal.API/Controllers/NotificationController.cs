using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VendorPortal.API.Data;

namespace VendorPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;

        public NotificationController(VendorPortalDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("Notifications/ProjectHead/{projectHeadId}")]
        public async Task<IActionResult> GetProjectHeadNotifications(string projectHeadId)
        {
            try
            {
                var notifications = await dbContext.NotificationsProjectHead
                    .Where(n => n.ProjectHeadId == projectHeadId)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching notifications.");
            }
        }


        [HttpGet]
        [Route("Admin/Notifications")]
        public async Task<IActionResult> GetAdminNotifications()
        {
            try
            {
                var notifications = await dbContext.AdminNotifications
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching notifications.");
            }
        }

        [HttpGet]
        [Route("Vendor/Notifications/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(string userId)
        {
            try
            {
                var notifications = await dbContext.VendorNotifications
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching notifications.");
            }
        }



    }
}
