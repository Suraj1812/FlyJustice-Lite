using System.ComponentModel.DataAnnotations;
using FlyJusticeLite.Models;
using FlyJusticeLite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages.Claims;

public sealed class TrackModel : PageModel
{
    private readonly IClaimService _claims;

    public TrackModel(IClaimService claims)
    {
        _claims = claims;
    }

    [BindProperty(SupportsGet = true)]
    [Required(ErrorMessage = "Enter a claim number.")]
    [RegularExpression(@"^FJL-\d{8}-\d{4}$", ErrorMessage = "Enter a claim number like FJL-20260615-1001.")]
    [Display(Name = "Claim Number")]
    public string? ClaimNumber { get; set; }

    public Claim? Claim { get; private set; }

    public bool Searched { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(ClaimNumber))
        {
            await LoadClaimAsync(cancellationToken);
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage(new { claimNumber = ClaimNumber?.Trim() });
    }

    private async Task LoadClaimAsync(CancellationToken cancellationToken)
    {
        Searched = true;
        Claim = await _claims.GetClaimByNumberAsync(ClaimNumber!.Trim(), cancellationToken);
    }
}
