using System.Security.Claims;
using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages.Admin;

[AllowAnonymous]
public sealed class LoginModel : PageModel
{
    private readonly IAdminAuthService _adminAuth;

    public LoginModel(IAdminAuthService adminAuth)
    {
        _adminAuth = adminAuth;
    }

    [BindProperty]
    public AdminLoginInput Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Admin/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var isValid = await _adminAuth.ValidateCredentialsAsync(
            Input.Username,
            Input.Password,
            cancellationToken);

        if (!isValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
            TempData["Toast.Type"] = "error";
            TempData["Toast.Message"] = "Sign in failed.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, Input.Username.Trim()),
            new(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTimeOffset.UtcNow
            });

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = "Signed in.";

        if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        {
            return LocalRedirect(ReturnUrl);
        }

        return RedirectToPage("/Admin/Index");
    }
}
