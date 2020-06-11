using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.App.Data;
using WebAdvert.App.Models.Auth;

namespace WebAdvert.App.Pages.Auth
{
    public class ConfirmModel : PageModel
    {
        private readonly AuthService _authService;

        public ConfirmModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public ConfirmViewModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var isValid = ModelState.IsValid && await ExecuteAsync();
            if (isValid)
            {
                return RedirectToPage("/auth/signin");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var result = await _authService.ConfirmUser(Input);

            if (result.Object || result.Errors == null || !result.Errors.Any())
                return result.Object;

            foreach (var error in result.Errors)
            {
                var propName = error.Contains("code", StringComparison.OrdinalIgnoreCase) ? "Code" : "Email";
                ModelState.AddModelError($"Input.{propName}", error);
            }

            return result.Object;
        }
    }
}