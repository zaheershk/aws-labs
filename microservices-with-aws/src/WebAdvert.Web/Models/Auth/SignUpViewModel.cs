using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Auth
{
    public class SignUpViewModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters!")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(8, ErrorMessage = "Password must be atleast 8 characters long!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match!")]
        public string ConfirmPassword { get; set; }
    }
}
