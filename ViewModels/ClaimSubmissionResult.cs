namespace FlyJusticeLite.ViewModels;

public sealed class ClaimSubmissionResult
{
    private ClaimSubmissionResult(bool succeeded, string? claimNumber, IReadOnlyList<string> errors)
    {
        Succeeded = succeeded;
        ClaimNumber = claimNumber;
        Errors = errors;
    }

    public bool Succeeded { get; }

    public string? ClaimNumber { get; }

    public IReadOnlyList<string> Errors { get; }

    public static ClaimSubmissionResult Success(string claimNumber) => new(true, claimNumber, []);

    public static ClaimSubmissionResult Failure(IReadOnlyList<string> errors) => new(false, null, errors);
}
