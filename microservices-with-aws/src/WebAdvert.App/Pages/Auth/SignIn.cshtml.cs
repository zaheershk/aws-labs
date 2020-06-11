using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.App.Data;
using WebAdvert.App.Models.Auth;

namespace WebAdvert.App.Pages.Auth
{
    public class SignInModel : PageModel
    {
        private readonly AuthService _authService;

        public SignInModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public SignInViewModel Input { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var isValid = ModelState.IsValid && await ExecuteAsync();
            if (isValid)
            {
                return RedirectToPage("/");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var result = await _authService.LoginUser(Input);

            if (!result.Object)
            {
                if (result.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Email", error);
                    }
                }
            }

            return result.Object;
        }
    }
}