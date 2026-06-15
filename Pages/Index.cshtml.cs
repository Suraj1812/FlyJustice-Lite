using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace FlyJusticeLite.Pages;

[EnableRateLimiting("public-form")]
public sealed class IndexModel : PageModel
{
    private readonly ICompensationCalculator _calculator;
    private readonly IClaimService _claims;
    private readonly IPublicContentService _content;

    public IndexModel(
        ICompensationCalculator calculator,
        IClaimService claims,
        IPublicContentService content)
    {
        _calculator = calculator;
        _claims = claims;
        _content = content;
    }

    [BindProperty]
    public EligibilityInput Input { get; set; } = new();

    public EligibilityResult? Result { get; private set; }

    public PublicClaimStats Stats { get; private set; } = PublicClaimStats.Empty;

    public IReadOnlyList<ServiceFeature> Services { get; private set; } = [];

    public IReadOnlyList<ProcessStep> ProcessSteps { get; private set; } = [];

    public IReadOnlyList<TrustPoint> TrustPoints { get; private set; } = [];

    public IReadOnlyList<SupportedAirline> SupportedAirlines { get; private set; } = [];

    public IReadOnlyList<PassengerStory> PassengerStories { get; private set; } = [];

    public IReadOnlyList<SuccessStory> SuccessStories { get; private set; } = [];

    public IReadOnlyList<FaqItem> FaqPreview { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        LoadContent();
        Stats = await _claims.GetPublicStatsAsync(cancellationToken);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        LoadContent();
        Stats = await _claims.GetPublicStatsAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Result = _calculator.Calculate(Input.DelayMinutes);
        return Page();
    }

    private void LoadContent()
    {
        Services = _content.GetServiceFeatures().Take(4).ToList();
        ProcessSteps = _content.GetProcessSteps();
        TrustPoints = _content.GetTrustPoints();
        SupportedAirlines = _content.GetSupportedAirlines();
        PassengerStories = _content.GetPassengerStories();
        SuccessStories = _content.GetSuccessStories();
        FaqPreview = _content.GetFaqCategories()
            .SelectMany(category => category.Items)
            .Take(4)
            .ToList();
    }
}
