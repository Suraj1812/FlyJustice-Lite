namespace FlyJusticeLite.Models;

public sealed class Claim
{
    public int Id { get; set; }

    public string ClaimNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string FlightNumber { get; set; } = string.Empty;

    public string Airline { get; set; } = string.Empty;

    public string DepartureAirport { get; set; } = string.Empty;

    public string ArrivalAirport { get; set; } = string.Empty;

    public int DelayMinutes { get; set; }

    public decimal CompensationAmount { get; set; }

    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ClaimDocument> Documents { get; set; } = new List<ClaimDocument>();
}
