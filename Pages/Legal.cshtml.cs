using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class LegalModel : PageModel
{
    private readonly IPublicContentService _content;

    public LegalModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<PublicContentPage> Pages { get; private set; } = [];

    public PublicContentPage? PageContent { get; private set; }

    public IActionResult OnGet(string slug)
    {
        Pages = _content.GetLegalPages();
        PageContent = _content.GetLegalPage(slug);

        return PageContent is null ? NotFound() : Page();
    }
}
