using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace FlyJusticeLite.Pages;

[EnableRateLimiting("public-form")]
public sealed class ContactModel : PageModel
{
    [BindProperty]
    public ContactInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            TempData["Toast.Type"] = "error";
            TempData["Toast.Message"] = "Please fix the highlighted fields.";
            return Page();
        }

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = "Your message has been received.";
        return RedirectToPage();
    }
}
