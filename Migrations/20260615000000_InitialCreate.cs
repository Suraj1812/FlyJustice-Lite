using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlyJusticeLite.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Claims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClaimNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                FullName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                FlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Airline = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                DepartureAirport = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                ArrivalAirport = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                DelayMinutes = table.Column<int>(type: "int", nullable: false),
                CompensationAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Claims", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Documents",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClaimId = table.Column<int>(type: "int", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                FilePath = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Documents", x => x.Id);
                table.ForeignKey(
                    name: "FK_Documents_Claims_ClaimId",
                    column: x => x.ClaimId,
                    principalTable: "Claims",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Claims_ClaimNumber",
            table: "Claims",
            column: "ClaimNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Claims_CreatedAt",
            table: "Claims",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Claims_Status",
            table: "Claims",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Documents_ClaimId",
            table: "Documents",
            column: "ClaimId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Documents");
        migrationBuilder.DropTable(name: "Claims");
    }
}
