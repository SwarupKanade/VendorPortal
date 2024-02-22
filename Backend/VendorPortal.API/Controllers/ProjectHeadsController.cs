using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.Dto;

namespace VendorPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectHeadsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;

        public ProjectHeadsController(UserManager<UserProfile> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] string Id)
        {
            UserProfile user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return BadRequest("Error, Invalid Id !!");
            }
            var userDto = new ProjectHeadDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return Ok(userDto);
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> Update([FromRoute] string Id, [FromBody] ProjectHeadUpdateDto projectHead)
        {
            UserProfile user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return BadRequest("Error, Invalid Id !!");
            }
 
            user.PhoneNumber = projectHead.PhoneNumber;
            user.Name = projectHead.Name;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }



    }
}
