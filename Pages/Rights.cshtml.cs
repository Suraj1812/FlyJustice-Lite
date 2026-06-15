using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class RightsModel : PageModel
{
    private readonly IPublicContentService _content;

    public RightsModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<PublicContentPage> Pages { get; private set; } = [];

    public PublicContentPage? PageContent { get; private set; }

    public IActionResult OnGet(string? slug)
    {
        Pages = _content.GetRightsPages();

        if (string.IsNullOrWhiteSpace(slug))
        {
            return Page();
        }

        PageContent = _content.GetRightsPage(slug.Trim());

        return PageContent is null ? NotFound() : Page();
    }
}
