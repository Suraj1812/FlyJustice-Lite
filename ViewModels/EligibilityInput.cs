using System.ComponentModel.DataAnnotations;

namespace FlyJusticeLite.ViewModels;

public sealed class EligibilityInput
{
    [Required, StringLength(20)]
    [RegularExpression(@"^[A-Za-z0-9]{2,3}\s?\d{1,4}[A-Za-z]?$", ErrorMessage = "Enter a valid flight number, for example BA492.")]
    [Display(Name = "Flight Number")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Airline { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Departure Airport")]
    public string DepartureAirport { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Arrival Airport")]
    public string ArrivalAirport { get; set; } = string.Empty;

    [Range(0, 1440, ErrorMessage = "Delay minutes must be between 0 and 1440.")]
    [Display(Name = "Delay Minutes")]
    public int DelayMinutes { get; set; }
}
