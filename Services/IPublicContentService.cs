using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public interface IPublicContentService
{
    IReadOnlyList<ServiceFeature> GetServiceFeatures();

    IReadOnlyList<ProcessStep> GetProcessSteps();

    IReadOnlyList<TrustPoint> GetTrustPoints();

    IReadOnlyList<PublicContentPage> GetRightsPages();

    PublicContentPage? GetRightsPage(string slug);

    IReadOnlyList<FaqCategory> GetFaqCategories();

    IReadOnlyList<FeeItem> GetFees();

    IReadOnlyList<PublicContentPage> GetLegalPages();

    PublicContentPage? GetLegalPage(string slug);
}
