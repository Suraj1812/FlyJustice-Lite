namespace FlyJusticeLite.ViewModels;

public sealed record PublicClaimStats(
    int TotalClaims,
    int ApprovedClaims,
    decimal PotentialCompensation,
    int AverageDelayMinutes)
{
    public static PublicClaimStats Empty { get; } = new(0, 0, 0, 0);
}
