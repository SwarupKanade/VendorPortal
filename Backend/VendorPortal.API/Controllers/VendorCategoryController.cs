using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorCategoryController : ControllerBase
    {

        private readonly VendorPortalDbContext dbContext;

        public VendorCategoryController(VendorPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] VendorCategoryDto vendorCategoryDto)
        {

            var vendorCategory = new VendorCategory
            {
                Name = vendorCategoryDto.Name,
                Description = vendorCategoryDto.Description,
                DocumentList = new List<VendorCategoryDocument>()
            };

            await dbContext.VendorCategories.AddAsync(vendorCategory);
            await dbContext.SaveChangesAsync();

            foreach (var docId in vendorCategoryDto.DocumentList)
            {
                var doc = await dbContext.Documents.FirstOrDefaultAsync(d => d.Id == docId);

                if (doc != null)
                {
                    vendorCategory.DocumentList.Add(new VendorCategoryDocument { Document = doc });
                }
                else
                {
                    return BadRequest($"Document with ID {docId} not found");
                }
            }

            await dbContext.SaveChangesAsync();

            return Ok(vendorCategory);
        }



        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var vendorCategoryResult = await dbContext.VendorCategories.Include(x=>x.DocumentList).ToListAsync();

            if (vendorCategoryResult != null) {
                
                return Ok(vendorCategoryResult);
            }

            return BadRequest("Something went wrong");
                
        }

    }
}