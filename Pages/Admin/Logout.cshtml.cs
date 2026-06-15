using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages.Admin;

public sealed class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = "Signed out.";
        return RedirectToPage("/Index");
    }
}
