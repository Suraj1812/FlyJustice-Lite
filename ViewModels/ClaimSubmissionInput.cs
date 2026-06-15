using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FlyJusticeLite.ViewModels;

public sealed class ClaimSubmissionInput : IValidatableObject
{
    [Required, StringLength(120, MinimumLength = 2)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress(ErrorMessage = "Enter a valid email address."), StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(32)]
    [RegularExpression(@"^\+?[0-9()\-\s]{7,24}$", ErrorMessage = "Enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [RegularExpression(@"^[A-Za-z0-9]{2,3}\s?\d{1,4}[A-Za-z]?$", ErrorMessage = "Enter a valid flight number, for example BA492.")]
    [Display(Name = "Flight Number")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string Airline { get; set; } = string.Empty;

    [Required, StringLength(80, MinimumLength = 2)]
    [Display(Name = "Departure Airport")]
    public string DepartureAirport { get; set; } = string.Empty;

    [Required, StringLength(80, MinimumLength = 2)]
    [Display(Name = "Arrival Airport")]
    public string ArrivalAirport { get; set; } = string.Empty;

    [Range(0, 1440, ErrorMessage = "Delay minutes must be between 0 and 1440.")]
    [Display(Name = "Delay Minutes")]
    public int DelayMinutes { get; set; }

    [Required]
    [Display(Name = "Ticket Upload")]
    public IFormFile? TicketUpload { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(DepartureAirport) &&
            string.Equals(
                DepartureAirport.Trim(),
                ArrivalAirport?.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "Departure and arrival airports must be different.",
                [nameof(ArrivalAirport)]);
        }
    }
}
