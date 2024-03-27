using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.GRNDto;
using VendorPortal.API.Models.DTO.RFPDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RFPController : ControllerBase
    {

        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<UserProfile> userManager;

        public RFPController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, UserManager<UserProfile> userManager)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromForm] RFPDto rfpDto)
        {
            ValidateFileUpload(rfpDto.Document);

            if (ModelState.IsValid)
            {
                string docPath = await Upload(rfpDto.Document);

                var rfp = new RFP
                {
                    Title = rfpDto.Title,
                    Document = docPath,
                    ProjectId = rfpDto.ProjectId,
                    VendorCategoryId = rfpDto.VendorCategoryId,
                    EndDate = rfpDto.EndDate,
                };

                await dbContext.RFPs.AddAsync(rfp);
                await dbContext.SaveChangesAsync();

                // Notify the admin
                await AddAdminNotification($"RFP '{rfpDto.Title}' is created.", "/rfp");


                // Notify users with the specified VendorCategoryId
                var usersToNotify = await dbContext.Users.Where(u => u.VendorCategoryId == rfpDto.VendorCategoryId).ToListAsync();
                foreach (var user in usersToNotify)
                {
                    await AddVendorNotification(user.Id, $"RFP '{rfpDto.Title}' is created.", "/");
                }

                // Get the project associated with the project ID
                var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == rfpDto.ProjectId);
                if (project != null && project.ProjectHeadId != null)
                {
                    // Notify the project head and redirect to /assigned-project route
                    await AddProjectHeadNotification(project.ProjectHeadId, $"RFP '{rfpDto.Title}' is created.", "/assigned-project");
                }

                return Ok(rfp);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var rfpResult = await dbContext.RFPs.Include("VendorCategory").Include("Project").FirstOrDefaultAsync(x => x.Id == id);

            if (rfpResult != null)
            {
                return Ok(rfpResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterVal)
        {

            var rfpsResult = dbContext.RFPs.Include("VendorCategory").Include("Project").AsQueryable();

            if (String.IsNullOrWhiteSpace(filterOn) == false && String.IsNullOrWhiteSpace(filterVal) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    rfpsResult = rfpsResult.Where(x => x.Title.ToLower().Contains(filterVal.ToLower()));
                }

                if (filterOn.Equals("category", StringComparison.OrdinalIgnoreCase))
                {
                    rfpsResult = rfpsResult.Where(x => x.VendorCategory.Name.ToLower().Contains(filterVal.ToLower()));
                }
            }

            if (rfpsResult != null)
            {
                return Ok(rfpsResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("AllActive")]
        public async Task<IActionResult> GetAllActive([FromQuery] string? filterOn, [FromQuery] string? filterVal)
        {
            var rfpsResult = dbContext.RFPs.Include("VendorCategory").Include("Project").AsEnumerable().Where(x => x.IsActive);

            if (String.IsNullOrWhiteSpace(filterOn) == false && String.IsNullOrWhiteSpace(filterVal) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    rfpsResult = rfpsResult.Where(x => x.Title.ToLower().Contains(filterVal.ToLower()));
                }

                if (filterOn.Equals("category", StringComparison.OrdinalIgnoreCase))
                {
                    rfpsResult = rfpsResult.Where(x => x.VendorCategory.Name.ToLower().Contains(filterVal.ToLower()));
                }
            }

            if (rfpsResult != null)
            {
                return Ok(rfpsResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("VendorCategory/{id:Guid}")]
        public async Task<IActionResult> GetAllByVendorCategory([FromRoute] Guid id)
        {
            var rfpsResult = await dbContext.RFPs.Include("VendorCategory").Include("Project").Where(x => x.VendorCategoryId == id).ToListAsync();

            if (rfpsResult != null)
            {
                return Ok(rfpsResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] RFPUpdateDto rfpUpdateDto)
        {
            var rfpResult = await dbContext.RFPs.FirstOrDefaultAsync(x => x.Id == id);

            if (rfpResult != null)
            {
                rfpResult.EndDate = rfpUpdateDto.EndDate;

                if (rfpUpdateDto.Document != null)
                {
                    ValidateFileUpload(rfpUpdateDto.Document);

                    if (ModelState.IsValid)
                    {
                        bool del = Delete(rfpResult.Document);
                        if (del)
                        {
                            string docPath = await Upload(rfpUpdateDto.Document);
                            rfpResult.Document = docPath;
                        }
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }

                await dbContext.SaveChangesAsync();
                return Ok(rfpResult);
            }

            return BadRequest("Something went wrong");
        }


        private async Task AddAdminNotification(string content, string route)
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
                    Content = content,
                    Route = route, // Set the appropriate route for the notification
                    CreatedAt = DateTime.Now
                };

                await dbContext.AdminNotifications.AddAsync(adminNotification);
            }

            await dbContext.SaveChangesAsync();
        }


        private async Task AddVendorNotification(string userId, string content, string route)
        {
            var vendorNotification = new NotificationVendor
            {
                UserId = userId,
                Content = content,
                Route = route, // Set the appropriate route for the project head notification
                CreatedAt = DateTime.Now
            };

            await dbContext.VendorNotifications.AddAsync(vendorNotification);
            await dbContext.SaveChangesAsync();
        }

        private async Task AddProjectHeadNotification(string projectHeadId, string content, string route)
        {
            var projectHeadNotification = new NotificationProjectHead
            {
                ProjectHeadId = projectHeadId,
                Content = content,
                Route = route, // Set the appropriate route for the project head notification
                CreatedAt = DateTime.Now
            };

            await dbContext.NotificationsProjectHead.AddAsync(projectHeadNotification);
            await dbContext.SaveChangesAsync();
        }

        private async Task<string> Upload(IFormFile document)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "RFPDocuments");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(document.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await document.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/RFPDocuments/{uniqueName}{fileExt}";

            var FilePath = urlFilePath;

            return FilePath;
        }

        private void ValidateFileUpload(IFormFile document)
        {
            var allowedExtensions = new string[] { ".pdf" };

            if (!allowedExtensions.Contains(Path.GetExtension(document.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (document.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private bool Delete(string filePath)
        {
            if (filePath != null)
            {
                string[] files = filePath.Split("/");
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "RFPDocuments", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}
