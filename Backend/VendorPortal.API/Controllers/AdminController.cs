using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VendorPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using VendorPortal.API.Models.DTO.AdminDto;
using VendorPortal.API.Models.DTO;
using VendorPortal.API.Data;

namespace VendorPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly VendorPortalDbContext dbContext;
        private readonly UserManager<UserProfile> userManager;

        public AdminController(VendorPortalDbContext dbContext, UserManager<UserProfile> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AdminDto adminDto)
        {

            var adminProfile = new UserProfile
            {
                UserName = adminDto.Email,
                Name = adminDto.Name,
                PhoneNumber = adminDto.PhoneNumber,
                Email = adminDto.Email,
                IsVerified = true,

            };

            var adminResult = await userManager.CreateAsync(adminProfile, adminDto.Password);

            if (adminResult.Succeeded)
            {
                List<string> roles = new List<string>();
                roles.Add("Admin");
                var adminRoleResult = await userManager.AddToRolesAsync(adminProfile, roles);
                if (adminRoleResult.Succeeded)
                {
                    return Ok("Admin was registered! Please login.");
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("ChangePassword/{Id:Guid}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string Id, [FromBody] ChangePasswordDto passwordDto)
        {
            var admin = await userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (passwordDto.CurrentPassword == passwordDto.NewPassword)
            {
                return BadRequest("Old and New Password are same, Please enter new password.");
            }
            else if (passwordDto.NewPassword != passwordDto.ConfirmPassword)
            {
                return BadRequest("New Password and Confirm Password are mis-matching.");
            }
            else
            {
                var passResult = await userManager.ChangePasswordAsync(admin, passwordDto.CurrentPassword, passwordDto.NewPassword);
                if (passResult.Succeeded)
                {
                    return Ok("Password Changed");
                }
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var allAdminResult = await userManager.GetUsersInRoleAsync("Admin");
            allAdminResult = allAdminResult.Where(x => x.Id == id).ToList();

            if (allAdminResult.Any())
            {
                var adminResult = await userManager.FindByIdAsync(id);
            
                var admin = new AdminResponseDto
                {
                    Id = adminResult.Id,
                    Name = adminResult.Name,
                    PhoneNumber = adminResult.PhoneNumber,
                    Email = adminResult.Email,
                    
                };
                return Ok(admin);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var adminsResult = await userManager.GetUsersInRoleAsync("Admin");
            if (adminsResult != null)
            {
                List<AdminResponseDto> allAdmins = new List<AdminResponseDto>();

                foreach (var singleAdmin in adminsResult)
                {
                    var admin = new AdminResponseDto
                    {
                        Id = singleAdmin.Id,
                        Name = singleAdmin.Name,
                        Email = singleAdmin.Email,
                        PhoneNumber = singleAdmin.PhoneNumber,
                    };

                    allAdmins.Add(admin);
                }

                return Ok(allAdmins);
            }

            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userResult = await dbContext.Users.ToListAsync();
            if (userResult != null)
            {
                List<UserResponseDto> allUsers = new List<UserResponseDto>();

                foreach (var singleUser in userResult)
                {
                    var admin = new UserResponseDto
                    {
                        Id = singleUser.Id,
                        Name = singleUser.Name,
                        UserName = singleUser.UserName,
                        OrganizationName = singleUser.OrganizationName,
                        Address = singleUser.Address,
                        City = singleUser.City,
                        State = singleUser.State,
                        Pincode = singleUser.Pincode,
                        Email = singleUser.Email,
                        PhoneNumber = singleUser.PhoneNumber,
                    };

                    allUsers.Add(admin);
                }

                return Ok(allUsers);
            }

            return BadRequest("Something went wrong");
        }

    }
}
