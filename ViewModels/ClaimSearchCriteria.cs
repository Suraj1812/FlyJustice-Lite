namespace FlyJusticeLite.ViewModels;

public sealed class ClaimSearchCriteria
{
    public string? Query { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
