using FlyJusticeLite.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyJusticeLite.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Claim> Claims => Set<Claim>();

    public DbSet<ClaimDocument> Documents => Set<ClaimDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.ToTable("Claims");
            entity.HasKey(claim => claim.Id);

            entity.HasIndex(claim => claim.ClaimNumber).IsUnique();
            entity.HasIndex(claim => claim.CreatedAt);
            entity.HasIndex(claim => claim.Status);

            entity.Property(claim => claim.ClaimNumber).HasMaxLength(32).IsRequired();
            entity.Property(claim => claim.FullName).HasMaxLength(120).IsRequired();
            entity.Property(claim => claim.Email).HasMaxLength(160).IsRequired();
            entity.Property(claim => claim.Phone).HasMaxLength(32).IsRequired();
            entity.Property(claim => claim.FlightNumber).HasMaxLength(20).IsRequired();
            entity.Property(claim => claim.Airline).HasMaxLength(120).IsRequired();
            entity.Property(claim => claim.DepartureAirport).HasMaxLength(80).IsRequired();
            entity.Property(claim => claim.ArrivalAirport).HasMaxLength(80).IsRequired();
            entity.Property(claim => claim.CompensationAmount).HasPrecision(10, 2);
            entity.Property(claim => claim.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
            entity.Property(claim => claim.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasMany(claim => claim.Documents)
                .WithOne(document => document.Claim)
                .HasForeignKey(document => document.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ClaimDocument>(entity =>
        {
            entity.ToTable("Documents");
            entity.HasKey(document => document.Id);

            entity.HasIndex(document => document.ClaimId);
            entity.Property(document => document.FileName).HasMaxLength(260).IsRequired();
            entity.Property(document => document.FilePath).HasMaxLength(1024).IsRequired();
            entity.Property(document => document.UploadedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}
