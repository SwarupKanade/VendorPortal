using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.GRNDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GRNController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GRNController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] GRNDto grnDto)
        {

            ValidateFileUpload(grnDto.Document);

            if (ModelState.IsValid)
            {
                string docPath = await Upload(grnDto.Document);

                var grn = new GRN
                {
                    GRNNo = grnDto.GRNNo,
                    PurchaseOrderId = grnDto.PurchaseOrderId,
                    SendOn = DateTime.Now,
                    ShipmentStatus = grnDto.ShipmentStatus,
                    DocumentPath = docPath,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                    Comment = "Created GRN"
                };

                await dbContext.GRNs.AddAsync(grn);
                await dbContext.SaveChangesAsync();
                return Ok(grn);
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
            var grnResult = await dbContext.GRNs.Include(x => x.PurchaseOrder).FirstOrDefaultAsync(x => x.Id == id);

            if (grnResult != null)
            {
                return Ok(grnResult);
            }
            return BadRequest("Something went wrong");
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var grnResult = await dbContext.GRNs.Include(x => x.PurchaseOrder).ToListAsync();

            if (grnResult != null)
            {
                return Ok(grnResult);
            }

            return BadRequest("Something went wrong");

        }


        [HttpGet]
        [Route("PurchaseOrder/{id:Guid}")]
        public async Task<IActionResult> GetByPurchaseOrderId([FromRoute] Guid id)
        {
            var grnResult = await dbContext.GRNs.Include(x => x.PurchaseOrder).Where(x => x.PurchaseOrderId == id).ToListAsync();

            if (grnResult != null)
            {
                return Ok(grnResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("Vendor/{id:Guid}")]
        public async Task<IActionResult> GetByVendorId([FromRoute] string id)
        {
            var grnResult = await dbContext.GRNs.Include(x => x.PurchaseOrder).Where(x => x.PurchaseOrder.VendorId == id).ToListAsync();

            if (grnResult != null)
            {
                return Ok(grnResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("AcceptReject/{id:Guid}")]
        public async Task<IActionResult> AcceptReject([FromRoute] Guid id, [FromBody] GRNProjectHeadUpdateDto grnProjectHeadUpdateDto)
        {
            var grnResult = await dbContext.GRNs.FirstOrDefaultAsync(x => x.Id == id);

            if (grnResult != null && grnResult.IsAccepted == null)
            {
                
                if (grnProjectHeadUpdateDto.IsAccepted)
                {
                    grnResult.Comment = grnProjectHeadUpdateDto.Comment != null ? grnProjectHeadUpdateDto.Comment : "Accepted";
                    grnResult.IsAccepted = grnProjectHeadUpdateDto.IsAccepted;
                    grnResult.AcceptedOn = DateTime.Now;
                }
                else
                {
                    grnResult.Comment = grnProjectHeadUpdateDto.Comment != null ? grnProjectHeadUpdateDto.Comment : "Rejected";
                    grnResult.IsAccepted = grnProjectHeadUpdateDto.IsAccepted;
                }

                grnResult.LastModifiedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(grnResult);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] GRNUpdateDto grnUpdateDto)
        {
            var grnResult = await dbContext.GRNs.FirstOrDefaultAsync(x => x.Id == id);

            if (grnResult != null)
            {
                grnResult.GRNNo = grnUpdateDto.GRNNo;
                grnResult.ShipmentStatus = grnUpdateDto.ShipmentStatus;
                grnResult.Comment = "Update";
                grnResult.LastModifiedOn = DateTime.Now;

                if (grnResult.IsAccepted == false)
                {
                    // After Rejection Edit GRN for Reapply
                    grnResult.IsAccepted = null;
                    grnResult.AcceptedOn = null;
                }

                if (grnUpdateDto.Document != null)
                {
                    ValidateFileUpload(grnUpdateDto.Document);

                    if (ModelState.IsValid)
                    {
                        bool del = Delete(grnResult.DocumentPath);
                        if (del)
                        {
                            string docPath = await Upload(grnUpdateDto.Document);
                            grnResult.DocumentPath = docPath;
                        }
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                await dbContext.SaveChangesAsync();
                return Ok(grnResult);

            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var grnResult = await dbContext.GRNs.FirstOrDefaultAsync(x => x.Id == id);

            if (grnResult == null)
            {
                return NotFound();
            }

            dbContext.GRNs.Remove(grnResult);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "GRNs");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/GRNs/{uniqueName}{fileExt}";
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
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "GRNs", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}