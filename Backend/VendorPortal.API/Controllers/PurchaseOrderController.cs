using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.PurchaseOrderDto;
using VendorPortal.API.Models.DTO.PurchaseOrderResponseDto;
using VendorPortal.API.Models.DTO.PurchaseOrderVendorResponseDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PurchaseOrderController(VendorPortalDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("Add")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromForm] PurchaseOrderDto purchaseOrderDto)
        {

            ValidateFileUpload(purchaseOrderDto.Document);

            if (ModelState.IsValid)
            {
                string docPath = await Upload(purchaseOrderDto.Document);

                var purchaseOrder = new PurchaseOrder
                {
                    OrderNo = purchaseOrderDto.OrderNo,
                    VendorId = purchaseOrderDto.VendorId,
                    ProjectId = purchaseOrderDto.ProjectId,
                    ReleaseDate = DateTime.Now,
                    ExpectedDelivery = purchaseOrderDto.ExpectedDelivery,
                    OrderAmount = purchaseOrderDto.OrderAmount,
                    DocumentPath = docPath,
                    IsActive = purchaseOrderDto.IsActive,
                    CreatedOn = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                    Comment = "Created Purchase Order"
                };

                await dbContext.PurchaseOrders.AddAsync(purchaseOrder);
                await dbContext.SaveChangesAsync();
                return Ok(purchaseOrder);
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
            var purchaseOrder = await dbContext.PurchaseOrders.Include(x => x.Vendor).Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id);

            if (purchaseOrder != null)
            {
                var allGRNResult = await dbContext.GRNs.Where(x => x.PurchaseOrderId == purchaseOrder.Id).ToListAsync();
                var GRNCount = allGRNResult.Count();
                var invoiceStatus = allGRNResult.All(x => x.InvoiceStatus == true);
                var newpurchaseOrder = new PurchaseOrderResponseDto
                {
                    Id = purchaseOrder.Id,
                    OrderNo = purchaseOrder.OrderNo,
                    VendorId = purchaseOrder.VendorId,
                    VendorName = purchaseOrder.Vendor.Name,
                    OrganizationName = purchaseOrder.Vendor.OrganizationName,
                    ReleaseDate = purchaseOrder.ReleaseDate,
                    ExpectedDelivery = purchaseOrder.ExpectedDelivery,
                    DocumentPath = purchaseOrder.DocumentPath,
                    OrderAmount = purchaseOrder.OrderAmount,
                    TotalGRN = GRNCount,
                    InvoiceStatus = invoiceStatus,
                    IsAccepted = purchaseOrder.IsAccepted,
                    AcceptedOn = purchaseOrder.AcceptedOn,
                    IsActive = purchaseOrder.IsActive,
                    CreatedOn = purchaseOrder.CreatedOn,
                    Comment = purchaseOrder.Comment,
                    Project = purchaseOrder.Project,
                };
                return Ok(newpurchaseOrder);
            }
            return BadRequest("Something went wrong");
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.Include(x => x.Vendor).Include(x => x.Project).ToListAsync();

            if (purchaseOrderResult != null)
            {
                List<PurchaseOrderResponseDto> allResult = new List<PurchaseOrderResponseDto>();
                foreach (var purchaseOrder in purchaseOrderResult)
                {
                    var allGRNResult = await dbContext.GRNs.Where(x => x.PurchaseOrderId == purchaseOrder.Id).ToListAsync();
                    var GRNCount = allGRNResult.Count();
                    var invoiceStatus = allGRNResult.All(x => x.InvoiceStatus == true);
                    var newpurchaseOrder = new PurchaseOrderResponseDto
                    {
                        Id = purchaseOrder.Id,
                        OrderNo = purchaseOrder.OrderNo,
                        VendorId = purchaseOrder.VendorId,
                        VendorName = purchaseOrder.Vendor.Name,
                        OrganizationName = purchaseOrder.Vendor.OrganizationName,
                        ReleaseDate = purchaseOrder.ReleaseDate,
                        ExpectedDelivery = purchaseOrder.ExpectedDelivery,
                        DocumentPath = purchaseOrder.DocumentPath,
                        OrderAmount = purchaseOrder.OrderAmount,
                        TotalGRN = GRNCount,
                        InvoiceStatus = invoiceStatus,
                        IsAccepted = purchaseOrder.IsAccepted,
                        AcceptedOn = purchaseOrder.AcceptedOn,
                        IsActive = purchaseOrder.IsActive,
                        CreatedOn = purchaseOrder.CreatedOn,
                        Comment = purchaseOrder.Comment,
                        Project = purchaseOrder.Project,
                    };
                    allResult.Add(newpurchaseOrder);
                }

                return Ok(allResult);
            }

            return BadRequest("Something went wrong");

        }

        [HttpGet]
        [Route("AllActive")]
        public async Task<IActionResult> GetActive()
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.Include(x => x.Vendor).Include(x => x.Project).Where(x => x.IsActive).ToListAsync();

            if (purchaseOrderResult != null)
            {
                List<PurchaseOrderResponseDto> allResult = new List<PurchaseOrderResponseDto>();
                foreach (var purchaseOrder in purchaseOrderResult)
                {
                    var allGRNResult = await dbContext.GRNs.Where(x => x.PurchaseOrderId == purchaseOrder.Id).ToListAsync();
                    var GRNCount = allGRNResult.Count();
                    var invoiceStatus = allGRNResult.All(x => x.InvoiceStatus == true);
                    var newpurchaseOrder = new PurchaseOrderResponseDto
                    {
                        Id = purchaseOrder.Id,
                        OrderNo = purchaseOrder.OrderNo,
                        VendorId = purchaseOrder.VendorId,
                        VendorName = purchaseOrder.Vendor.Name,
                        OrganizationName = purchaseOrder.Vendor.OrganizationName,
                        ReleaseDate = purchaseOrder.ReleaseDate,
                        ExpectedDelivery = purchaseOrder.ExpectedDelivery,
                        DocumentPath = purchaseOrder.DocumentPath,
                        OrderAmount = purchaseOrder.OrderAmount,
                        TotalGRN = GRNCount,
                        InvoiceStatus = invoiceStatus,
                        IsAccepted = purchaseOrder.IsAccepted,
                        AcceptedOn = purchaseOrder.AcceptedOn,
                        IsActive = purchaseOrder.IsActive,
                        CreatedOn = purchaseOrder.CreatedOn,
                        Comment = purchaseOrder.Comment,
                        Project = purchaseOrder.Project
                    };
                    allResult.Add(newpurchaseOrder);
                }
                return Ok(allResult);
            }

            return BadRequest("Something went wrong");
        }


        [HttpGet]
        [Route("Vendor/{id:Guid}")]
        public async Task<IActionResult> GetByVendorId([FromRoute] string id)
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.Include(x => x.Vendor).Include(x => x.Project).Where(x => x.IsActive && x.VendorId == id).ToListAsync();

            if (purchaseOrderResult != null)
            {
                List<PurchaseOrderVendorResponseDto> allResult = new List<PurchaseOrderVendorResponseDto>();
                foreach (var purchaseOrder in purchaseOrderResult)
                {
                    var newpurchaseOrder = new PurchaseOrderVendorResponseDto
                    {
                        Id = purchaseOrder.Id,
                        OrderNo = purchaseOrder.OrderNo,
                        VendorId = purchaseOrder.VendorId,
                        VendorName = purchaseOrder.Vendor.Name,
                        OrganizationName = purchaseOrder.Vendor.OrganizationName,
                        ReleaseDate = purchaseOrder.ReleaseDate,
                        ExpectedDelivery = purchaseOrder.ExpectedDelivery,
                        OrderAmount = purchaseOrder.OrderAmount,
                        IsAccepted = purchaseOrder.IsAccepted,
                        DocumentPath = purchaseOrder.DocumentPath,
                        Project = purchaseOrder.Project,
                        Comment = purchaseOrder.Comment,
                    };
                    allResult.Add(newpurchaseOrder);
                }

                return Ok(allResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("Project/{id:Guid}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] Guid id)
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.Include(x => x.Vendor).Include(x => x.Project).Where(x => x.IsActive && x.ProjectId == id).ToListAsync();

            if (purchaseOrderResult != null)
            {
                List<PurchaseOrderVendorResponseDto> allResult = new List<PurchaseOrderVendorResponseDto>();
                foreach (var purchaseOrder in purchaseOrderResult)
                {
                    var newpurchaseOrder = new PurchaseOrderVendorResponseDto
                    {
                        Id = purchaseOrder.Id,
                        OrderNo = purchaseOrder.OrderNo,
                        VendorId = purchaseOrder.VendorId,
                        VendorName = purchaseOrder.Vendor.Name,
                        OrganizationName = purchaseOrder.Vendor.OrganizationName,
                        ReleaseDate = purchaseOrder.ReleaseDate,
                        ExpectedDelivery = purchaseOrder.ExpectedDelivery,
                        OrderAmount = purchaseOrder.OrderAmount,
                        IsAccepted = purchaseOrder.IsAccepted,
                        DocumentPath = purchaseOrder.DocumentPath,
                        Project = purchaseOrder.Project,
                        Comment = purchaseOrder.Comment,
                    };
                    allResult.Add(newpurchaseOrder);
                }

                return Ok(allResult);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("AcceptReject/{id:Guid}")]
        public async Task<IActionResult> AcceptReject([FromRoute] Guid id, [FromBody] PurchaseOrderVendorUpdateDto purchaseOrderVendorUpdateDto)
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

            if (purchaseOrderResult != null && purchaseOrderResult.IsAccepted == null)
            {
                //Versioning
                var history = purchaseOrderResult.replicate();
                dbContext.PurchaseOrderHistories.Add(history);
                purchaseOrderResult.PreviousRevisionId = history.Id;

                if (purchaseOrderVendorUpdateDto.IsAccepted)
                {
                    purchaseOrderResult.Comment = purchaseOrderVendorUpdateDto.Comment != null ? purchaseOrderVendorUpdateDto.Comment : "Accepted";
                    purchaseOrderResult.IsAccepted = purchaseOrderVendorUpdateDto.IsAccepted;
                    purchaseOrderResult.AcceptedOn = DateTime.Now;
                }
                else
                {
                    purchaseOrderResult.Comment = purchaseOrderVendorUpdateDto.Comment;
                    purchaseOrderResult.IsAccepted = purchaseOrderVendorUpdateDto.IsAccepted;
                }

                purchaseOrderResult.LastModifiedOn = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return Ok(purchaseOrderResult);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] PurchaseOrderUpdateDto purchaseOrderUpdateDto)
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

            if (purchaseOrderResult != null)
            {
                if (purchaseOrderResult.IsAccepted == null || purchaseOrderResult.IsAccepted == false)
                {
                    //Versioning
                    var history = purchaseOrderResult.replicate();
                    dbContext.PurchaseOrderHistories.Add(history);
                    purchaseOrderResult.PreviousRevisionId = history.Id;

                    purchaseOrderResult.OrderNo = purchaseOrderUpdateDto.OrderNo;
                    purchaseOrderResult.ExpectedDelivery = purchaseOrderUpdateDto.ExpectedDelivery;
                    purchaseOrderResult.OrderAmount = purchaseOrderUpdateDto.OrderAmount;
                    purchaseOrderResult.IsActive = purchaseOrderUpdateDto.IsActive;
                    purchaseOrderResult.Comment = "Update";
                    purchaseOrderResult.LastModifiedOn = DateTime.Now;


                    // After Rejection Edit Purchase Order for Reapply
                    purchaseOrderResult.IsAccepted = null;
                    purchaseOrderResult.AcceptedOn = null;

                    if (purchaseOrderUpdateDto.Document != null)
                    {
                        ValidateFileUpload(purchaseOrderUpdateDto.Document);

                        if (ModelState.IsValid)
                        {
                            string docPath = await Upload(purchaseOrderUpdateDto.Document);
                            purchaseOrderResult.DocumentPath = docPath;
                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    return Ok(purchaseOrderResult);
                }
                else
                {
                    return BadRequest("Already Accepted So Unable to Update");
                }

            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var purchaseOrderResult = await dbContext.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

            if (purchaseOrderResult == null)
            {
                return NotFound();
            }

            Delete(purchaseOrderResult.DocumentPath);
            dbContext.PurchaseOrders.Remove(purchaseOrderResult);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [Route("History/{id:Guid}")]
        public async Task<IActionResult> GetHistory([FromRoute] Guid id)
        {
            var mainResult = await dbContext.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);

            if (mainResult != null)
            {
                List<PurchaseOrderHistory> allResult = new List<PurchaseOrderHistory>();
                if (mainResult.PreviousRevisionId == null)
                {
                    return Ok(allResult); //No History
                }
                var historyResult = await dbContext.PurchaseOrderHistories.FirstOrDefaultAsync(x => x.Id == mainResult.PreviousRevisionId);
                allResult.Add(historyResult);
                while (historyResult.PreviousRevisionId != null)
                {
                    historyResult = await dbContext.PurchaseOrderHistories.FirstOrDefaultAsync(x => x.Id == historyResult.PreviousRevisionId);
                    allResult.Add(historyResult);
                }
                return Ok(allResult);
            }

            return BadRequest("Something went wrong");
        }

        private async Task<string> Upload(IFormFile image)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "PurchaseOrders");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(image.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/PurchaseOrders/{uniqueName}{fileExt}";
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
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "PurchaseOrders", files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

    }
}