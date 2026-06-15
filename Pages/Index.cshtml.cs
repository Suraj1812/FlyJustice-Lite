using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class IndexModel : PageModel
{
    private readonly ICompensationCalculator _calculator;
    private readonly IClaimService _claims;

    public IndexModel(ICompensationCalculator calculator, IClaimService claims)
    {
        _calculator = calculator;
        _claims = claims;
    }

    [BindProperty]
    public EligibilityInput Input { get; set; } = new();

    public EligibilityResult? Result { get; private set; }

    public PublicClaimStats Stats { get; private set; } = PublicClaimStats.Empty;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Stats = await _claims.GetPublicStatsAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        Stats = await _claims.GetPublicStatsAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Result = _calculator.Calculate(Input.DelayMinutes);
        return Page();
    }
}
