using System.ComponentModel.DataAnnotations;

namespace WebAdvert.App.Models.Auth
{
    public class ConfirmModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}
