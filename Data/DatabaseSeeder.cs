using FlyJusticeLite.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyJusticeLite.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.Claims.AnyAsync())
        {
            return;
        }

        var createdAt = DateTime.UtcNow;

        var claims = new[]
        {
            new Claim
            {
                ClaimNumber = "FJL-20260615-1001",
                FullName = "Amelia Wright",
                Email = "amelia.wright@example.com",
                Phone = "+44 7700 900101",
                FlightNumber = "BA492",
                Airline = "British Airways",
                DepartureAirport = "London Heathrow",
                ArrivalAirport = "Gibraltar",
                DelayMinutes = 315,
                CompensationAmount = 400,
                Status = ClaimStatus.UnderReview,
                CreatedAt = createdAt.AddDays(-5)
            },
            new Claim
            {
                ClaimNumber = "FJL-20260615-1002",
                FullName = "Noah Meyer",
                Email = "noah.meyer@example.com",
                Phone = "+49 151 23456789",
                FlightNumber = "LH1034",
                Airline = "Lufthansa",
                DepartureAirport = "Frankfurt",
                ArrivalAirport = "Paris Charles de Gaulle",
                DelayMinutes = 190,
                CompensationAmount = 250,
                Status = ClaimStatus.Pending,
                CreatedAt = createdAt.AddDays(-2)
            },
            new Claim
            {
                ClaimNumber = "FJL-20260615-1003",
                FullName = "Sofia Rossi",
                Email = "sofia.rossi@example.com",
                Phone = "+39 320 555 0198",
                FlightNumber = "AZ708",
                Airline = "ITA Airways",
                DepartureAirport = "Rome Fiumicino",
                ArrivalAirport = "Delhi",
                DelayMinutes = 640,
                CompensationAmount = 600,
                Status = ClaimStatus.Approved,
                CreatedAt = createdAt.AddDays(-9)
            }
        };

        await dbContext.Claims.AddRangeAsync(claims);
        await dbContext.SaveChangesAsync();
    }
}
