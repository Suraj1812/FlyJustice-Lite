using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public sealed class CompensationCalculator : ICompensationCalculator
{
    public EligibilityResult Calculate(int delayMinutes)
    {
        if (delayMinutes < 180)
        {
            return new EligibilityResult(false, 0, "This delay is below the 180 minute threshold.");
        }

        if (delayMinutes < 300)
        {
            return new EligibilityResult(true, 250, "This delay may qualify for EUR 250 compensation.");
        }

        if (delayMinutes < 600)
        {
            return new EligibilityResult(true, 400, "This delay may qualify for EUR 400 compensation.");
        }

        return new EligibilityResult(true, 600, "This delay may qualify for EUR 600 compensation.");
    }
}
