using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EventController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] EventDto eventDto)
        {

            ValidateFileUpload(eventDto.Image);

            if (ModelState.IsValid)
            {
                string imgPath = await Upload(eventDto.Image);

                var newEvent = new Event
                {
                    Title = eventDto.Title,
                    ImagePath = imgPath,
                    EventDateTime = eventDto.EventDateTime,
                    Content = eventDto.Content,
                    IsActive = eventDto.IsActive,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                };

                await dbContext.Events.AddAsync(newEvent);
                await dbContext.SaveChangesAsync();
                return Ok(newEvent);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var eventResult = await dbContext.Events.ToListAsync();

            if (eventResult != null)
            {
                return Ok(eventResult);
            }

            return BadRequest("Something went wrong");

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] EventDto eventDto)
        {
            var eventResult = await dbContext.Events.FirstOrDefaultAsync(x => x.Id == id);

            if (eventResult != null)
            {
                eventResult.Title = eventDto.Title;
                eventResult.EventDateTime = eventDto.EventDateTime;
                eventResult.Content = eventDto.Content;
                eventResult.IsActive = eventDto.IsActive;
                eventResult.LastModified = DateTime.Now;

                if (eventDto.Image != null)
                {
                    ValidateFileUpload(eventDto.Image);

                    if (ModelState.IsValid)
                    {
                        bool del = await Delete(eventResult.ImagePath);
                        if (del)
                        {
                            string imgPath = await Upload(eventDto.Image);
                            eventResult.ImagePath = imgPath;
                        }
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }

                }
                await dbContext.SaveChangesAsync();
                return Ok(eventResult);

            }
            return BadRequest("Something went wrong");
        }


        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "Events");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/Events/{uniqueName}{fileExt}";
            var FilePath = urlFilePath;
            return FilePath;
        }
        private void ValidateFileUpload(IFormFile image)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (image.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private async Task<bool> Delete(string filePath)
        {
            if (filePath != null)
            {
                string[] files = filePath.Split("/");
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "Events", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}