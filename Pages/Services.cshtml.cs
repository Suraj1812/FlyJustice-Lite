using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages;

public sealed class ServicesModel : PageModel
{
    private readonly IPublicContentService _content;

    public ServicesModel(IPublicContentService content)
    {
        _content = content;
    }

    public IReadOnlyList<ServiceFeature> Services { get; private set; } = [];

    public IReadOnlyList<ProcessStep> ProcessSteps { get; private set; } = [];

    public void OnGet()
    {
        Services = _content.GetServiceFeatures();
        ProcessSteps = _content.GetProcessSteps();
    }
}
