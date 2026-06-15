using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class FeesModel : PageModel
{
    private readonly IPublicContentService _content;

    public FeesModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<FeeItem> Fees { get; private set; } = [];

    public void OnGet()
    {
        Fees = _content.GetFees();
    }
}
