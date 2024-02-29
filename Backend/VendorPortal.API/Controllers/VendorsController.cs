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
    public class VendorsController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;

        public VendorsController(UserManager<UserProfile> userManager)
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
            var userDto = new VendorDto
            {
                Organization = user.Organization,
                ContactPerson = user.ContactPerson,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Category = user.Category,
                State =user.State,
                City =user.City,
                Address = user.Address
            };
            return Ok(userDto);
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> Update([FromRoute] string Id, [FromBody] VendorUpdateDto vendorDto)
        {
            UserProfile user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return BadRequest("Error, Invalid Id !!");
            }
            user.Organization = vendorDto.Organization;
            user.ContactPerson = vendorDto.ContactPerson;
            user.PhoneNumber = vendorDto.PhoneNumber;
            user.Category = vendorDto.Category;
            user.State = vendorDto.State;
            user.City = vendorDto.City;
            user.Address = vendorDto.Address;
            user.Pincode = vendorDto.Pincode;
            user.Category = vendorDto.Category;
            
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
