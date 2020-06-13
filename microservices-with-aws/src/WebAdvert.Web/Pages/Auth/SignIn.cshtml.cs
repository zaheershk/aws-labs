using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.Web.Services;
using WebAdvert.Web.Models.Auth;

namespace WebAdvert.Web.Pages.Auth
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
                return RedirectToPage("/Index");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var result = await _authService.LoginUser(Input);

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