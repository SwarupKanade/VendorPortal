using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using User.Management.Services.Models;
using User.Management.Services.Services;
using VendorPortal.API.Data;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.Dto;

namespace VendorPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]         
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserProfile> userManager;
        private readonly VendorPortalDbContext dbContext;
        private readonly IEmailServices _IEmailServices;

        public AuthController(UserManager<UserProfile> userManager,VendorPortalDbContext dbContext, IEmailServices IEmailServices)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this._IEmailServices = IEmailServices;
        }

        
        [HttpPost]
        [Route("Register/Vendor")]
        public async Task<IActionResult> VendorRegistration([FromBody] VendorDto vendorDto)
        {
            var vendor = new UserProfile
            {
                Name = vendorDto.Organization,
                UserName = vendorDto.Organization,
                Organization = vendorDto.Organization,
                ContactPerson = vendorDto.ContactPerson,
                Email = vendorDto.Email,
                PhoneNumber = vendorDto.PhoneNumber,
                State = vendorDto.State,
                City = vendorDto.City,
                Address = vendorDto.Address,
                Pincode = vendorDto.Pincode,
                Category = vendorDto.Category
            };

            var vendorResult=await userManager.CreateAsync(vendor, vendorDto.Password);

            if (vendorResult.Succeeded)
            {
                string []roles = ["Vendor"];
                vendorResult = await userManager.AddToRolesAsync(vendor, roles);
                if (vendorResult.Succeeded)
                {
                    UserProfile user= await userManager.FindByEmailAsync(vendorDto.Email);
                    string vendorId = await userManager.GetUserIdAsync(user);

                    var vendorCategory = new VendorCategory
                    {
                        VendorId = vendorId,
                        CategoryId = vendorDto.Category
                    };

                    await dbContext.VendorCategories.AddAsync(vendorCategory);
                    await dbContext.SaveChangesAsync();

                    //Send Email to vendor
                    string email = $@"Email: {vendorDto.Email}";
                    string password = vendorDto.Password;

                    // Constructing the message using string interpolation
                    string messageBody = $"Email: {email} && Password: {password}";
                    var message = new Message(new string[] { vendorDto.Email }, "Login Credentials", messageBody);

                    _IEmailServices.SendMail(message);

                    return Ok(" Vendor is added !! ");
                }
                return BadRequest("Error, Roles is not added !!");
             }

            return BadRequest(vendorResult);
        }

        [HttpPost]
        [Route("Register/ProjectHead")]
        public async Task<IActionResult> ProjectHeadRegistration([FromBody] ProjectHeadDto projectHeadDto)
        {
            var projectHead = new UserProfile
            {
                Name = projectHeadDto.Name,
                UserName = projectHeadDto.UserName,
                Email = projectHeadDto.Email,
                PhoneNumber = projectHeadDto.PhoneNumber 
            };
            var response = await userManager.CreateAsync(projectHead,projectHeadDto.Password);
            if (response.Succeeded)
            {
                string[] roles = ["ProjectHead"];
                response = await userManager.AddToRolesAsync(projectHead,roles);
                if (response.Succeeded)
                {
                    //Send Email to Project Head

                    string email = $@"Email: {projectHeadDto.Email}";
                    string password = projectHeadDto.Password;

                    // Constructing the message using string interpolation
                    string messageBody = $"Email: {projectHeadDto.Email} && Password: {password}";


                    var message = new Message(new string[] { projectHeadDto.Email }, "Login Credentials", messageBody);

                    _IEmailServices.SendMail(message);
                    
                    return Ok("Project Head is Created !! ");
                }
                return BadRequest("Can not Assign Role to Project Head !! ");
            }
            return BadRequest("Error, Can not create Project Head !!");
        }


        [HttpPost]
        [Route("Login/Vendor")]
        public async Task<IActionResult> VendorLogin([FromBody] VendorLoginDto vendorLogin)
        {
            UserProfile vendor = await userManager.FindByEmailAsync(vendorLogin.Email);
            

            if(vendor == null)
            {
                return BadRequest("No Records Found !! ");
            }
            var response = await userManager.CheckPasswordAsync(vendor, vendorLogin.Password);
            if (response)
            {
                return Ok("Vendor is Looged In !!");
            }
            return BadRequest("Unable to Log In !!");

        }


        [HttpPost]
        [Route("Login/ProjectHead")]
        public async Task<IActionResult> ProjectLoginLogin([FromBody] ProjectHeadLoginDto projectHead)
        {
            UserProfile head = await userManager.FindByEmailAsync(projectHead.Email);

            if (head == null)
            {
                return BadRequest("No Records Found !! ");
            }
            var response = await userManager.CheckPasswordAsync(head, projectHead.Password);
            if (response)
            {
                return Ok("Project Head is Looged In !!");
            }
            return BadRequest("Unable to Log In !!");

        }

        [HttpPost]
        [Route("Create/Project")]
        public async Task<IActionResult> CreateProject([FromBody]ProjectDto projectDto)
        {


            var project = new Project
            {
                ProjectHeadId = projectDto.ProjectHeadId,
                ProjectHeadName = projectDto.ProjectHeadName,
                Name = projectDto.Name,
                ProjectStatus = projectDto.ProjectStatus,
                CreatedOn = projectDto.CreatedOn,
                Description = projectDto.Description
            };

            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return Ok(projectDto);
        }

    }
}
