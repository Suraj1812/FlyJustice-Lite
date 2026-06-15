namespace FlyJusticeLite.Options;

public sealed class SiteOptions
{
    public const string SectionName = "Site";

    public string Name { get; set; } = "FlyJustice Lite";

    public string PublicBaseUrl { get; set; } = string.Empty;

    public string MetaDescription { get; set; } =
        "Check eligibility, submit flight delay compensation claims, and track claim status with FlyJustice Lite.";
}
