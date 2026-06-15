using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FlyJusticeLite.ViewModels;

public sealed class ClaimSubmissionInput
{
    [Required, StringLength(120)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(32)]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

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

    [Required]
    [Display(Name = "Ticket Upload")]
    public IFormFile? TicketUpload { get; set; }
}
