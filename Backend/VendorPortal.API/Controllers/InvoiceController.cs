using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.InvoiceDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InvoiceController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] InvoiceDto invoiceDto)
        {

            ValidateFileUpload(invoiceDto.Document);

            if (ModelState.IsValid)
            {
                string docPath = await Upload(invoiceDto.Document);

                var invoice = new Invoice
                {
                    InvoiceNo = invoiceDto.InvoiceNo,
                    SendOn = DateTime.Now,
                    Amount = invoiceDto.Amount,
                    GRNId = invoiceDto.GRNId,
                    PaymentStatus = invoiceDto.PaymentStatus,
                    DueDate = invoiceDto.DueDate,
                    DocumentPath = docPath,
                    Comment = "Created Invoice",
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                };

                await dbContext.Invoices.AddAsync(invoice);
                await dbContext.SaveChangesAsync();
                return Ok(invoice);
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
            var invoiceResult = await dbContext.Invoices.Include(x => x.GRN).ThenInclude(x => x.PurchaseOrder).FirstOrDefaultAsync(x => x.Id == id);

            if (invoiceResult != null)
            {
                return Ok(invoiceResult);
            }
            return BadRequest("Something went wrong");
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var invoiceResult = await dbContext.Invoices.Include(x => x.GRN).ThenInclude(x => x.PurchaseOrder).ToListAsync();

            if (invoiceResult != null)
            {
                return Ok(invoiceResult);
            }
            return BadRequest("Something went wrong");

        }

        [HttpGet]
        [Route("GRN/{id:Guid}")]
        public async Task<IActionResult> GetByGRNId([FromRoute] Guid id)
        {
            var invoiceResult = await dbContext.Invoices.Include(x => x.GRN).ThenInclude(x => x.PurchaseOrder).Where(x => x.GRNId == id).ToListAsync();

            if (invoiceResult != null)
            {
                return Ok(invoiceResult);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("AcceptReject/{id:Guid}")]
        public async Task<IActionResult> AcceptReject([FromRoute] Guid id, [FromBody] InvoiceProjectHeadUpdateDto invoiceProjectHeadUpdateDto)
        {
            var invoiceResult = await dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoiceResult != null && invoiceResult.IsAccepted == null)
            {
                if (invoiceProjectHeadUpdateDto.IsAccepted)
                {
                    invoiceResult.Comment = invoiceProjectHeadUpdateDto.Comment != null ? invoiceProjectHeadUpdateDto.Comment : "Accepted";
                    invoiceResult.IsAccepted = invoiceProjectHeadUpdateDto.IsAccepted;
                    invoiceResult.AcceptedOn = DateTime.Now;
                }
                else
                {
                    invoiceResult.Comment = invoiceProjectHeadUpdateDto.Comment != null ? invoiceProjectHeadUpdateDto.Comment : "Rejected";
                    invoiceResult.IsAccepted = invoiceProjectHeadUpdateDto.IsAccepted;
                }

                invoiceResult.LastModifiedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(invoiceResult);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] InvoiceUpdateDto invoiceUpdateDto)
        {
            var invoiceResult = await dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoiceResult != null)
            {
                invoiceResult.InvoiceNo = invoiceUpdateDto.InvoiceNo;
                invoiceResult.Amount = invoiceUpdateDto.Amount;
                invoiceResult.GRNId = invoiceUpdateDto.GRNId;
                invoiceResult.PaymentStatus = invoiceUpdateDto.PaymentStatus;
                invoiceResult.DueDate = invoiceUpdateDto.DueDate;
                invoiceResult.Comment = "Update";
                invoiceResult.LastModifiedOn = DateTime.Now;

                if (invoiceResult.IsAccepted == false)
                {
                    // After Rejection Edit Invoice for Reapply
                    invoiceResult.IsAccepted = null;
                    invoiceResult.AcceptedOn = null;
                }

                if (invoiceUpdateDto.Document != null)
                {
                    ValidateFileUpload(invoiceUpdateDto.Document);

                    if (ModelState.IsValid)
                    {
                        bool del = Delete(invoiceResult.DocumentPath);
                        if (del)
                        {
                            string docPath = await Upload(invoiceUpdateDto.Document);
                            invoiceResult.DocumentPath = docPath;
                        }
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                await dbContext.SaveChangesAsync();
                return Ok(invoiceResult);

            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var invoiceResult = await dbContext.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoiceResult == null)
            {
                return NotFound();
            }

            dbContext.Invoices.Remove(invoiceResult);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "Invoices");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/Invoices/{uniqueName}{fileExt}";
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
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "Invoices", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}