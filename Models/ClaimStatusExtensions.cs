namespace FlyJusticeLite.Models;

public static class ClaimStatusExtensions
{
    public static string ToDisplayName(this ClaimStatus status) => status switch
    {
        ClaimStatus.Pending => "Pending",
        ClaimStatus.UnderReview => "Under Review",
        ClaimStatus.Approved => "Approved",
        ClaimStatus.Rejected => "Rejected",
        _ => status.ToString()
    };

    public static string ToCssClass(this ClaimStatus status) => status switch
    {
        ClaimStatus.Pending => "status-pending",
        ClaimStatus.UnderReview => "status-review",
        ClaimStatus.Approved => "status-approved",
        ClaimStatus.Rejected => "status-rejected",
        _ => "status-pending"
    };
}
