using System.ComponentModel.DataAnnotations;

namespace VendorPortal.API.Models.DTO
{
    public class ChangePasswordDto
    {
        [Required]
        public String CurrentPassword { get; set; }

        [Required]
        public String NewPassword { get; set; }

        [Required]
        public String ConfirmPassword { get; set; }
    }
}
