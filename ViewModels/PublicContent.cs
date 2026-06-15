namespace FlyJusticeLite.ViewModels;

public sealed record ContentBlock(
    string Heading,
    string Body,
    IReadOnlyList<string>? Bullets = null);

public sealed record PublicContentPage(
    string Slug,
    string Title,
    string Eyebrow,
    string Summary,
    IReadOnlyList<string> Highlights,
    IReadOnlyList<ContentBlock> Sections,
    string CtaText = "Start your claim",
    string CtaPage = "/Claims/Submit");

public sealed record ServiceFeature(
    string Title,
    string Description,
    string LinkText,
    string Page,
    string? Slug = null);

public sealed record ProcessStep(
    string Number,
    string Title,
    string Description,
    bool IsPassengerTask);

public sealed record TrustPoint(
    string Title,
    string Description);

public sealed record FaqCategory(
    string Name,
    string Description,
    IReadOnlyList<FaqItem> Items);

public sealed record FaqItem(
    string Question,
    string Answer,
    IReadOnlyList<string>? Bullets = null);

public sealed record FeeItem(
    string Title,
    string Amount,
    string Description);

public sealed class ContactInput
{
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(120)]
    [System.ComponentModel.DataAnnotations.Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [System.ComponentModel.DataAnnotations.StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.StringLength(32)]
    [System.ComponentModel.DataAnnotations.RegularExpression(@"^$|^FJL-\d{8}-\d{4}$", ErrorMessage = "Enter a claim number like FJL-20260615-1001.")]
    [System.ComponentModel.DataAnnotations.Display(Name = "Claim Number")]
    public string? ClaimNumber { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.StringLength(1200, MinimumLength = 20)]
    public string Message { get; set; } = string.Empty;
}
