namespace FlyJusticeLite.ViewModels;

public sealed record EligibilityResult(bool IsEligible, decimal CompensationAmount, string Message);
