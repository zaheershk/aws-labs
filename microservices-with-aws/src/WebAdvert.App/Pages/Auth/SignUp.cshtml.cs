using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.App.Data;
using WebAdvert.App.Models.Auth;

namespace WebAdvert.App.Pages.Auth
{
    public class SignUpModel : PageModel
    {
        private readonly AuthService _authService;

        public SignUpModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public SignUpViewModel Input { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var isValid = ModelState.IsValid && await ExecuteAsync();
            if (isValid)
            {
                return RedirectToPage("/auth/confirm");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var result = await _authService.RegisterUser(Input);

            if (result.Object || result.Errors == null || !result.Errors.Any())
                return result.Object;

            foreach (var error in result.Errors)
            {
                var propName = error.Contains("password", StringComparison.OrdinalIgnoreCase) ? "Password" : "Email";
                ModelState.AddModelError($"Input.{propName}", error);
            }

            return result.Object;
        }
    }
}