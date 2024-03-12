using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NewsController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] NewsDto newsDto)
        {

            ValidateFileUpload(newsDto.Image);

            if (ModelState.IsValid)
            {
                string imgPath = await Upload(newsDto.Image);

                var news = new News
                {
                    Title = newsDto.Title,
                    ImagePath = imgPath,
                    Content = newsDto.Content,
                    IsActive = newsDto.IsActive,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                };

                await dbContext.Newss.AddAsync(news);
                await dbContext.SaveChangesAsync();
                return Ok(news);
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
            var newsResult = await dbContext.Newss.ToListAsync();

            if (newsResult != null)
            {
                return Ok(newsResult);
            }

            return BadRequest("Something went wrong");

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] NewsDto newsDto)
        {
            var newsResult = await dbContext.Newss.FirstOrDefaultAsync(x => x.Id == id);

            if (newsResult != null)
            {
                ValidateFileUpload(newsDto.Image);

                if (ModelState.IsValid)
                {
                    bool del = await Delete(newsResult.ImagePath);
                    if (del)
                    {
                        string imgPath = await Upload(newsDto.Image);
                        newsResult.ImagePath = imgPath;
                    }
                    newsResult.Title = newsDto.Title;
                    newsResult.Content = newsDto.Content;
                    newsResult.IsActive = newsDto.IsActive;
                    newsResult.LastModified = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                    return Ok(newsResult);

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest("Something went wrong");
        }


        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "News");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/News/{uniqueName}{fileExt}";
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
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "News", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}