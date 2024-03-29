using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO.ProductCategoryDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {

        private readonly VendorPortalDbContext dbContext;

        public ProductCategoryController(VendorPortalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ProductCategoryDto productCategoryDto)
        {

            var productCategory = new ProductCategory
            {
                Name = productCategoryDto.Name,
                IsRoot = productCategoryDto.IsRoot,
                ParentCategoryId = productCategoryDto?.ParentCategoryId,
                Description = productCategoryDto.Description,
                Status = productCategoryDto.Status
            };

            await dbContext.ProductCategories.AddAsync(productCategory);
            await dbContext.SaveChangesAsync();
            return Ok(productCategory);
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var productCategoryResult = await dbContext.ProductCategories.Include("ParentCategory")?.ToListAsync();

            if (productCategoryResult != null) {
                
                return Ok(productCategoryResult);
            }

            return BadRequest("Something went wrong");
                
        }

        [HttpGet]
        [Route("Parent")]
        public async Task<IActionResult> GetRoot()
        {
            var productCategoryResult = await dbContext.ProductCategories.Where(x => x.IsRoot).ToListAsync();

            if (productCategoryResult != null)
            {
                List<ProductCategory> allCategory = new List<ProductCategory>();
                foreach (var item in productCategoryResult)
                {
                    var productCategory = await dbContext.ProductCategories.Where(x => x.ParentCategoryId == item.Id).ToListAsync();

                    if (productCategory.Any())
                    {
                        allCategory.Add(item);
                    }
                }

                return Ok(allCategory);

            }
            
            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("Category/{id:Guid}")]
        public async Task<IActionResult> GetSubCategory([FromRoute] Guid id)
        {
            var productCategoryResult = await dbContext.ProductCategories.Where(x => x.ParentCategoryId == id).ToListAsync();

            if (productCategoryResult != null)
            {
                return Ok(productCategoryResult);
            }

            return BadRequest("Something went wrong");

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var productCategory = await dbContext.ProductCategories.Include("ParentCategory")?.FirstOrDefaultAsync(x => x.Id == id);

            if (productCategory != null)
            {
                return Ok(productCategory);
            }

            return BadRequest("Something went wrong");
        }

    }
}