using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public interface ICompensationCalculator
{
    EligibilityResult Calculate(int delayMinutes);
}
