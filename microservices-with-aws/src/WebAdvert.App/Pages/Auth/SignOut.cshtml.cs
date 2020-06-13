using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAdvert.App.Data;

namespace WebAdvert.App.Pages.Auth
{
    public class SignOutModel : PageModel
    {
        private readonly AuthService _authService;

        public SignOutModel(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var isValid = await ExecuteAsync();
            if (isValid)
            {
                return RedirectToPage("/auth/signin");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> ExecuteAsync()
        {
            var result = await _authService.LogoutUser();

            return result.Object;
        }
    }
}