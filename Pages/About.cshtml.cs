using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class AboutModel : PageModel
{
    private readonly IPublicContentService _content;

    public AboutModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<TrustPoint> TrustPoints { get; private set; } = [];

    public void OnGet()
    {
        TrustPoints = _content.GetTrustPoints();
    }
}
