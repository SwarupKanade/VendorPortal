using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.RFPApplication;
using VendorPortal.API.Models.DTO.RFPApplicationDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RFPApplicationController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RFPApplicationController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] RFPApplicationDto rfpApplicationDto)
        {

            ValidateFileUpload(rfpApplicationDto.Document);

            if (ModelState.IsValid)
            {
                string docPath = await Upload(rfpApplicationDto.Document);

                var rfpApp = new RFPApplication
                {
                    RFPId = rfpApplicationDto.RFPId,
                    VendorId = rfpApplicationDto.VendorId,
                    Comment = rfpApplicationDto.Comment,
                    DocumentPath = docPath,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };

                await dbContext.RFPApplications.AddAsync(rfpApp);
                await dbContext.SaveChangesAsync();
                return Ok(rfpApp);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var rfpAppResult = await dbContext.RFPApplications.Include(x => x.RFP).ThenInclude(x => x.VendorCategory).Include(x => x.Vendor).FirstOrDefaultAsync(x => x.Id == id);

            if (rfpAppResult != null)
            {
                var rfpApp = new RFPApplicationResponseDto
                {
                    Id = rfpAppResult.Id,
                    RFPId = rfpAppResult.RFPId,
                    VendorId = rfpAppResult.VendorId,
                    DocumentPath = rfpAppResult.DocumentPath,
                    Comment = rfpAppResult.Comment,
                    VendorName = rfpAppResult.Vendor.Name,
                    RFP = rfpAppResult.RFP,
                    CreatedOn = rfpAppResult.CreatedOn,
                    LastModifiedOn = rfpAppResult.LastModifiedOn,
                };
                return Ok(rfpApp);
            }

            return BadRequest("Something went wrong");

        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var rfpAppResult = await dbContext.RFPApplications.Include(x => x.RFP).ThenInclude(x => x.VendorCategory).Include(x => x.Vendor).ToListAsync();

            if (rfpAppResult != null)
            {
                List<RFPApplicationResponseDto> rfpApplications = new List<RFPApplicationResponseDto>();
                foreach (var rfpApp in rfpAppResult)
                {
                    var newRFPApp = new RFPApplicationResponseDto
                    {
                        Id = rfpApp.Id,
                        RFPId = rfpApp.RFPId,
                        VendorId = rfpApp.VendorId,
                        DocumentPath = rfpApp.DocumentPath,
                        Comment = rfpApp.Comment,
                        VendorName = rfpApp.Vendor.Name,
                        RFP = rfpApp.RFP,
                        CreatedOn = rfpApp.CreatedOn,
                        LastModifiedOn = rfpApp.LastModifiedOn,
                    };
                    rfpApplications.Add(newRFPApp);
                }
                return Ok(rfpApplications);
            }

            return BadRequest("Something went wrong");

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var rfpAppResult = await dbContext.RFPApplications.FirstOrDefaultAsync(x => x.Id == id);

            if (rfpAppResult == null)
            {
                return NotFound();
            }

            Delete(rfpAppResult.DocumentPath);
            dbContext.RFPApplications.Remove(rfpAppResult);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "RFPApplication");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/RFPApplication/{uniqueName}{fileExt}";
            var FilePath = urlFilePath;
            return FilePath;
        }
        private void ValidateFileUpload(IFormFile image)
        {
            var allowedExtensions = new string[] { ".pdf" };

            if (!allowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (image.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private bool Delete(string filePath)
        {
            if (filePath != null)
            {
                string[] files = filePath.Split("/");
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "RFPApplication", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}