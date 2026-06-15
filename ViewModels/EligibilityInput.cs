using System.ComponentModel.DataAnnotations;

namespace FlyJusticeLite.ViewModels;

public sealed class EligibilityInput
{
    [Required, StringLength(20)]
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

    [Range(0, 1440)]
    [Display(Name = "Delay Minutes")]
    public int DelayMinutes { get; set; }
}
