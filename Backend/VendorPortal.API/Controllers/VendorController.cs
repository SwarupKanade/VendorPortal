using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using VendorPortal.API.Data;
using VendorPortal.API.Mail;
using VendorPortal.API.Models.Domain;
using VendorPortal.API.Models.DTO;
using VendorPortal.API.Models.DTO.VendorDto;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {

        private readonly UserManager<UserProfile> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailService emailService;
        private readonly VendorPortalDbContext dbContext;

        public VendorController(UserManager<UserProfile> userManager, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, EmailService emailService, VendorPortalDbContext dbContext)
        {
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
            this.dbContext = dbContext;
        }


        [HttpPost]
        [Route("Register")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] VendorDto vendorDto)
        {
            var newVendor = new UserProfile
            {
                OrganizationName = vendorDto.OrganizationName,
                Name = vendorDto.Name,
                PhoneNumber = vendorDto.PhoneNumber,
                State = vendorDto.State,
                City = vendorDto.City,
                Address = vendorDto.Address,
                Pincode = vendorDto.Pincode,
                Email = vendorDto.Email,
                UserName = vendorDto.Email,
                VendorCategoryId = vendorDto.VendorCategoryId,
                DocumentsUploadList = new List<DocumentsUpload>(),
                IsVerified = false,
            };

            var vendorResult = await userManager.CreateAsync(newVendor, "Pass@123");

            if (vendorResult.Succeeded)
            {
                List<string> roles = new List<string>();
                roles.Add("Vendor");
                vendorResult = await userManager.AddToRolesAsync(newVendor, roles);

                if (vendorResult.Succeeded)
                {
                    var vendorCategory = dbContext.VendorCategories.Include(vc => vc.DocumentList).FirstOrDefault(vc => vc.Id == newVendor.VendorCategoryId);

                    if (vendorCategory != null)
                    {
                        foreach (VendorCategoryDocument document in vendorCategory.DocumentList)
                        {
                            var newDocumentsUploadStatus = new DocumentsUpload
                            {
                                VendorId = newVendor.Id,
                                DocumentId = document.DocumentId,
                                DocumentPath = null,
                                Comment = "Upload",
                                IsVerified = false
                            };
                            newVendor.DocumentsUploadList.Add(newDocumentsUploadStatus);
                            await dbContext.SaveChangesAsync();
                        }
                    }

                    // Send welcome notification
                    // Send welcome notification with route information
                    await AddVendorNotification(newVendor.Id, $"Welcome to SCIQUS Vendor Portal {newVendor.Name} , Click here to complete verification", "/upload-document");

                    // Send welcome notification
                    // Send welcome notification to admin with the appropriate route
                    await AddAdminNotification1(newVendor.Name, "/vendor-list");


                    SendWelcomeEmail(newVendor);
                    return Ok("Vendor was registered! Please login.");
                }

            }

            return BadRequest("Something went wrong");
        }

        private async Task AddAdminNotification1(string vendorName, string route)
        {
            var adminRole = "Admin"; // Assuming the role name for admin is "Admin"

            // Find the admin users with the specified role
            var adminUsers = await userManager.GetUsersInRoleAsync(adminRole);

            // Iterate through each admin user and send them the notification
            foreach (var adminUser in adminUsers)
            {
                var adminNotification = new NotificationAdmin
                {
                    AdminId = adminUser.Id,
                    Content = $"Vendor '{vendorName}' was registered.",
                    Route = route, // Set the appropriate route for the notification
                    CreatedAt = DateTime.Now
                };

                await dbContext.AdminNotifications.AddAsync(adminNotification);
            }

            await dbContext.SaveChangesAsync();
        }



        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var allVendorResult = await userManager.GetUsersInRoleAsync("Vendor");
            allVendorResult = allVendorResult.Where(x => x.Id == id).ToList();

            if (allVendorResult.Any())
            {
                var vendorResult = await dbContext.Users.Include(u => u.DocumentsUploadList).ThenInclude(du => du.Document).Include(u => u.VendorCategory).FirstOrDefaultAsync(x => x.Id == id);

                var vendor = new VendorResponseDto
                {
                    Id = vendorResult.Id,
                    OrganizationName = vendorResult.OrganizationName,
                    Name = vendorResult.Name,
                    Email = vendorResult.Email,
                    PhoneNumber = vendorResult.PhoneNumber,
                    State = vendorResult.State,
                    Address = vendorResult.Address,
                    Pincode = (int)vendorResult.Pincode,
                    City = vendorResult.City,
                    IsVerified = vendorResult.IsVerified,
                    DocumentsUploadList = vendorResult.DocumentsUploadList.Select(qi => new DocumentsUploadResponseDto
                    {
                        Id = qi.Id,
                        DocumentId = qi.Document.Id,
                        DocumentName = qi.Document.Name,
                        DocumentPath = qi.DocumentPath,
                        IsVerified = qi.IsVerified,
                        Comment = qi.Comment,
                    }).ToList(),
                    VendorCategory = vendorResult.VendorCategory,
                };

                return Ok(vendor);
            }

            return BadRequest("Something went wrong");
        }


        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll([FromQuery] string? nameVal, [FromQuery] string? orgVal,
            [FromQuery] string? catVal)
        {

            var dbVendorResult = await userManager.GetUsersInRoleAsync("Vendor");
            var vendorResult = dbVendorResult.AsQueryable();

            if (String.IsNullOrWhiteSpace(nameVal) == false)
            {
                vendorResult = vendorResult.Where(x => x.Name.ToLower().Contains(nameVal.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(orgVal) == false)
            {
                vendorResult = vendorResult.Where(x => x.OrganizationName.ToLower().Contains(orgVal.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(catVal) == false)
            {
                vendorResult = vendorResult.Where(x => x.VendorCategory.Name.ToLower().Contains(catVal.ToLower()));
            }


            if (vendorResult != null)
            {
                List<VendorResponseDto> allVendor = new List<VendorResponseDto>();
                foreach (var vendor in vendorResult)
                {
                    var singleVendor = await dbContext.Users.Include(u => u.DocumentsUploadList).ThenInclude(du => du.Document).Include(u => u.VendorCategory).FirstOrDefaultAsync(x => x.Id == vendor.Id);
                    var newVendor = new VendorResponseDto
                    {
                        Id = singleVendor.Id,
                        OrganizationName = singleVendor.OrganizationName,
                        Name = singleVendor.Name,
                        Email = singleVendor.Email,
                        PhoneNumber = singleVendor.PhoneNumber,
                        State = singleVendor.State,
                        Address = singleVendor.Address,
                        City = singleVendor.City,
                        Pincode = (int)singleVendor.Pincode,
                        IsVerified = singleVendor.IsVerified,
                        DocumentsUploadList = singleVendor.DocumentsUploadList.Select(qi => new DocumentsUploadResponseDto
                        {
                            Id = qi.Id,
                            DocumentId = qi.Document.Id,
                            DocumentName = qi.Document.Name,
                            DocumentPath = qi.DocumentPath,
                            IsVerified = qi.IsVerified,
                            Comment = qi.Comment,
                        }).ToList(),
                        VendorCategory = singleVendor.VendorCategory,
                    };

                    allVendor.Add(newVendor);
                }
                return Ok(allVendor);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("ChangePassword/{Id:Guid}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string Id, [FromBody] ChangePasswordDto passwordDto)
        {
            var vendor = await userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);
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
                var passResult = await userManager.ChangePasswordAsync(vendor, passwordDto.CurrentPassword, passwordDto.NewPassword);
                if (passResult.Succeeded)
                {
                    return Ok("Password Changed");
                }
            }
            return BadRequest("Error");
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] VendorUpdateDto vendorUpdateDto)
        {
            var allVendorResult = await userManager.GetUsersInRoleAsync("Vendor");
            allVendorResult = allVendorResult.Where(x => x.Id == id).ToList();

            if (allVendorResult.Any())
            {
                var vendorResult = await userManager.FindByIdAsync(id);

                if (vendorUpdateDto.NewPassword != "")
                {
                    var passResult = await userManager.ChangePasswordAsync(vendorResult, vendorUpdateDto.CurrentPassword, vendorUpdateDto.NewPassword);
                    if (!passResult.Succeeded)
                    {
                        return BadRequest(passResult.Errors);
                    }
                }

                vendorResult.OrganizationName = vendorUpdateDto.OrganizationName;
                vendorResult.Name = vendorUpdateDto.Name;
                vendorResult.PhoneNumber = vendorUpdateDto.PhoneNumber;
                vendorResult.City = vendorUpdateDto.City;
                vendorResult.State = vendorUpdateDto.State;
                vendorResult.Address = vendorUpdateDto.Address;
                vendorResult.Pincode = vendorUpdateDto.Pincode;

                await userManager.UpdateAsync(vendorResult);

                vendorResult = await dbContext.Users.Include(u => u.DocumentsUploadList).ThenInclude(du => du.Document).Include(u => u.VendorCategory).FirstOrDefaultAsync(x => x.Id == id);

                var vendor = new VendorResponseDto
                {
                    Id = vendorResult.Id,
                    OrganizationName = vendorResult.OrganizationName,
                    Name = vendorResult.Name,
                    Email = vendorResult.Email,
                    PhoneNumber = vendorResult.PhoneNumber,
                    State = vendorResult.State,
                    Address = vendorResult.Address,
                    City = vendorResult.City,
                    Pincode = (int)vendorResult.Pincode,
                    IsVerified = vendorResult.IsVerified,
                    DocumentsUploadList = vendorResult.DocumentsUploadList.Select(qi => new DocumentsUploadResponseDto
                    {
                        Id = qi.Id,
                        DocumentId = qi.Document.Id,
                        DocumentName = qi.Document.Name,
                        DocumentPath = qi.DocumentPath,
                        IsVerified = qi.IsVerified,
                        Comment = qi.Comment,
                    }).ToList(),
                    VendorCategory = vendorResult.VendorCategory,
                };

                return Ok(vendor);
            }


            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Doc")]
        public async Task<IActionResult> UpdateDoc([FromForm] VendorDocUpdateDto vendorDocUpdateDto)
        {
            var documentResult = await dbContext.DocumentsUploads.Include(x => x.Document).FirstOrDefaultAsync(x => x.Id == vendorDocUpdateDto.Id);

            if (documentResult != null)
            {
                ValidateFileUpload(vendorDocUpdateDto.Document);

                if (ModelState.IsValid)
                {
                    string docPath = await Upload(vendorDocUpdateDto.Document, documentResult.VendorId);

                    documentResult.DocumentPath = docPath;
                    documentResult.Comment = "Uploaded";

                    await dbContext.SaveChangesAsync();

                    var vendor = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == documentResult.VendorId);

                    if (vendor != null)
                    {
                        // Construct the appropriate route with the document ID
                        var route = $"/document-verification/{vendor.Id}";

                        // Send admin notification with the appropriate route
                        await AddAdminNotification($"{vendor.Name} (ID: {vendor.Id}) has uploaded a document for verification. Please click here to view.", route);
                    }


                    return Ok($"{documentResult.Document.Name} Uploaded");
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }

            return BadRequest("Something went wrong");
  
      }
        private async Task AddAdminNotification(string content, string route)
        {
            var adminRole = "Admin"; // Assuming the role name for admin is "Admin"

            // Find the admin users with the specified role
            var adminUsers = await userManager.GetUsersInRoleAsync(adminRole);

            // Iterate through each admin user and send them the notification
            foreach (var adminUser in adminUsers)
            {
                var adminNotification = new NotificationAdmin
                {
                    AdminId = adminUser.Id,
                    Content = content,
                    Route = route, // Set the appropriate route for the notification
                    CreatedAt = DateTime.Now
                };

                await dbContext.AdminNotifications.AddAsync(adminNotification);
            }

            await dbContext.SaveChangesAsync();
        }




        [HttpPost]
        [Route("DocVerify")]
        public async Task<IActionResult> DocVerify([FromBody] VendorDocVerifyDto vendorDocVerifyDto)
        {
            var documentResult = await dbContext.DocumentsUploads.Include(x => x.Document).FirstOrDefaultAsync(x => x.Id == vendorDocVerifyDto.Id);

            if (documentResult != null)
            {
                bool documentVerified = vendorDocVerifyDto.DocumentVerified;

                if (!documentVerified)
                {
                    // Document rejected
                    bool del = Delete(documentResult.DocumentPath);
                    if (del) { documentResult.DocumentPath = null; }
                    documentResult.Comment = vendorDocVerifyDto.Comment;
                }
                else
                {
                    // Document approved
                    documentResult.Comment = "Approved";
                }

                documentResult.IsVerified = documentVerified;

                var allDocs = await dbContext.DocumentsUploads.Where(x => x.VendorId == documentResult.VendorId).ToListAsync();

                if (allDocs != null)
                {
                    bool isVerify = true;
                    foreach (var doc in allDocs)
                    {
                        if (!doc.IsVerified)
                        {
                            isVerify = false;
                            break; // No need to continue checking if one document is not verified
                        }
                    }

                    var vendor = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == documentResult.VendorId);
                    vendor.IsVerified = isVerify;
                    await dbContext.SaveChangesAsync();

                    // Send appropriate notification based on verification status
                    string notificationMessage = documentVerified ?
                        $"Your document '{documentResult.Document.Name}' has been approved." :
                        $"Your document '{documentResult.Document.Name}' has been rejected. Please upload the document again.";
                    string route = "/upload-document"; // Route is the same for both cases

                    await AddVendorNotification(vendor.Id, notificationMessage, route);
                }

                await dbContext.SaveChangesAsync();

                return Ok("Completed");
            }

            return BadRequest("Something went wrong");
        }


        private async Task AddVendorNotification(string userId, string content, string route)
        {
            var vendorNotification = new NotificationVendor
            {
                UserId = userId,
                Content = content,
                Route = route, // Include the route associated with the notification
                CreatedAt = DateTime.Now
            };

            await dbContext.VendorNotifications.AddAsync(vendorNotification);
            await dbContext.SaveChangesAsync();
        }



        private async Task<string> Upload(IFormFile document, string id)
        {
            var folder = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "VendorDocuments", id);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string uniqueName = Guid.NewGuid().ToString();
            string fileExt = Path.GetExtension(document.FileName);
            var localFilePath = Path.Combine(folder, $"{uniqueName}{fileExt}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await document.CopyToAsync(stream);
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Files/VendorDocuments/{id}/{uniqueName}{fileExt}";
            var FilePath = urlFilePath;
            return FilePath;
        }

        private bool Delete(string filePath)
        {
            if (filePath != null)
            {
                string[] files = filePath.Split("/");
                string ExitingFile = Path.Combine(webHostEnvironment.ContentRootPath, "Files", "VendorDocuments", files[files.Length - 2], files[files.Length - 1]);
                System.IO.File.Delete(ExitingFile);
                return true;
            }
            return false;
        }

        private void ValidateFileUpload(IFormFile document)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".pdf" };

            if (!allowedExtensions.Contains(Path.GetExtension(document.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (document.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }

        private void SendWelcomeEmail(UserProfile user)
        {
            string subject = $"Welcome to Our Application Vendor ID {user.Id}";
            string body = $"Dear {user.Name},\n\nWelcome to our application! Your username is: {user.UserName} and your password is: Pass@123\n\nBest regards,\nYour Application Team";

            emailService.SendEmail(user.Email, subject, body);
        }

    }

}
