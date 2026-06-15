using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace FlyJusticeLite.Pages.Claims;

[EnableRateLimiting("public-form")]
public sealed class SubmitModel : PageModel
{
    private readonly IClaimService _claims;

    public SubmitModel(IClaimService claims)
    {
        _claims = claims;
    }

    [BindProperty]
    public ClaimSubmissionInput Input { get; set; } = new();

    public void OnGet(
        string? flightNumber,
        string? airline,
        string? departureAirport,
        string? arrivalAirport,
        int? delayMinutes)
    {
        Input.FlightNumber = flightNumber ?? string.Empty;
        Input.Airline = airline ?? string.Empty;
        Input.DepartureAirport = departureAirport ?? string.Empty;
        Input.ArrivalAirport = arrivalAirport ?? string.Empty;
        Input.DelayMinutes = delayMinutes ?? 0;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Toast.Type"] = "error";
            TempData["Toast.Message"] = "Please fix the highlighted fields.";
            return Page();
        }

        var result = await _claims.SubmitClaimAsync(Input, cancellationToken);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Input.TicketUpload", error);
            }

            TempData["Toast.Type"] = "error";
            TempData["Toast.Message"] = "We could not submit the claim yet.";
            return Page();
        }

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = $"Claim {result.ClaimNumber} was submitted.";
        return RedirectToPage("/Claims/Track", new { claimNumber = result.ClaimNumber });
    }
}
