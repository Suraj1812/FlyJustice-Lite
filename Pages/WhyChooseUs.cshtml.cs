using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class WhyChooseUsModel : PageModel
{
    private readonly IPublicContentService _content;

    public WhyChooseUsModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<TrustPoint> TrustPoints { get; private set; } = [];

    public void OnGet()
    {
        TrustPoints = _content.GetTrustPoints();
    }
}
