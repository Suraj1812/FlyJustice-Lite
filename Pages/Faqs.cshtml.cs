using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class FaqsModel : PageModel
{
    private readonly IPublicContentService _content;

    public FaqsModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<FaqCategory> Categories { get; private set; } = [];

    public void OnGet()
    {
        Categories = _content.GetFaqCategories();
    }
}
