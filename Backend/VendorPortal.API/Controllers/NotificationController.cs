using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace VendorPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly UserManager<UserProfile> userManager;

        public NotificationController(VendorPortalDbContext dbContext, UserManager<UserProfile> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            try
            {
                // Check if the user with the specified ID exists
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    return StatusCode((int)HttpStatusCode.NotFound, "User not found");

                // Check the roles of the user
                var userRoles = await userManager.GetRolesAsync(user);

                // Route the request based on user role
                if (userRoles.Contains("Admin"))
                {
                    var adminNotifications = await dbContext.AdminNotifications
                        .OrderByDescending(n => n.CreatedAt)
                        .ToListAsync();
                    return Ok(adminNotifications);
                }
                else if (userRoles.Contains("ProjectHead"))
                {
                    var projectHeadNotifications = await dbContext.NotificationsProjectHead
                        .Where(n => n.ProjectHeadId == userId)
                        .OrderByDescending(n => n.CreatedAt)
                        .ToListAsync();
                    return Ok(projectHeadNotifications);
                }
                else if (userRoles.Contains("Vendor"))
                {
                    var vendorNotifications = await dbContext.VendorNotifications
                        .Where(n => n.UserId == userId)
                        .OrderByDescending(n => n.CreatedAt)
                        .ToListAsync();
                    return Ok(vendorNotifications);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, "User does not have access to notifications");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while fetching notifications.");
            }
        }

        [HttpDelete]
        [Route("{userId}/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string userId, int notificationId)
        {
            try
            {
                // Check if the user with the specified ID exists
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    return StatusCode((int)HttpStatusCode.NotFound, "User not found");

                // Check the roles of the user
                var userRoles = await userManager.GetRolesAsync(user);

                // Route the request based on user role
                if (userRoles.Contains("Admin"))
                {
                    var notificationToDelete = await dbContext.AdminNotifications
                        .FirstOrDefaultAsync(n => n.Id == notificationId && n.AdminId == userId);

                    if (notificationToDelete == null)
                        return StatusCode((int)HttpStatusCode.NotFound, "Notification not found");

                    dbContext.AdminNotifications.Remove(notificationToDelete);
                }
                else if (userRoles.Contains("ProjectHead"))
                {
                    var notificationToDelete = await dbContext.NotificationsProjectHead
                        .FirstOrDefaultAsync(n => n.Id == notificationId && n.ProjectHeadId == userId);

                    if (notificationToDelete == null)
                        return StatusCode((int)HttpStatusCode.NotFound, "Notification not found");

                    dbContext.NotificationsProjectHead.Remove(notificationToDelete);
                }
                else if (userRoles.Contains("Vendor"))
                {
                    var notificationToDelete = await dbContext.VendorNotifications
                        .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

                    if (notificationToDelete == null)
                        return StatusCode((int)HttpStatusCode.NotFound, "Notification not found");

                    dbContext.VendorNotifications.Remove(notificationToDelete);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, "User does not have access to notifications");
                }

                await dbContext.SaveChangesAsync();

                return Ok("Notification deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the notification.");
            }
        }
    }
}
