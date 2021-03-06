﻿using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Auth
{
    public class ConfirmViewModel
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
