using System;
using FlyJusticeLite.Data;
using FlyJusticeLite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlyJusticeLite.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.16")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("FlyJusticeLite.Models.Claim", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Airline")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("nvarchar(120)");

            b.Property<string>("ArrivalAirport")
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnType("nvarchar(80)");

            b.Property<string>("ClaimNumber")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)");

            b.Property<decimal>("CompensationAmount")
                .HasPrecision(10, 2)
                .HasColumnType("decimal(10,2)");

            b.Property<DateTime>("CreatedAt")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            b.Property<int>("DelayMinutes")
                .HasColumnType("int");

            b.Property<string>("DepartureAirport")
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnType("nvarchar(80)");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(160)
                .HasColumnType("nvarchar(160)");

            b.Property<string>("FlightNumber")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("nvarchar(120)");

            b.Property<string>("Phone")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)");

            b.Property<ClaimStatus>("Status")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)")
                .HasConversion<string>();

            b.HasKey("Id");
            b.HasIndex("ClaimNumber").IsUnique();
            b.HasIndex("CreatedAt");
            b.HasIndex("Status");
            b.ToTable("Claims", (string)null);
        });

        modelBuilder.Entity("FlyJusticeLite.Models.ClaimDocument", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("ClaimId")
                .HasColumnType("int");

            b.Property<string>("FileName")
                .IsRequired()
                .HasMaxLength(260)
                .HasColumnType("nvarchar(260)");

            b.Property<string>("FilePath")
                .IsRequired()
                .HasMaxLength(1024)
                .HasColumnType("nvarchar(1024)");

            b.Property<DateTime>("UploadedAt")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            b.HasKey("Id");
            b.HasIndex("ClaimId");
            b.ToTable("Documents", (string)null);
        });

        modelBuilder.Entity("FlyJusticeLite.Models.ClaimDocument", b =>
        {
            b.HasOne("FlyJusticeLite.Models.Claim", "Claim")
                .WithMany("Documents")
                .HasForeignKey("ClaimId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Claim");
        });

        modelBuilder.Entity("FlyJusticeLite.Models.Claim", b =>
        {
            b.Navigation("Documents");
        });
#pragma warning restore 612, 618
    }
}
