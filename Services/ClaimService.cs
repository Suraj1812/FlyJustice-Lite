using System.Security.Cryptography;
using FlyJusticeLite.Models;
using FlyJusticeLite.Repositories;
using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public sealed class ClaimService : IClaimService
{
    private readonly IClaimRepository _claims;
    private readonly ICompensationCalculator _calculator;
    private readonly IFileStorageService _fileStorage;

    public ClaimService(
        IClaimRepository claims,
        ICompensationCalculator calculator,
        IFileStorageService fileStorage)
    {
        _claims = claims;
        _calculator = calculator;
        _fileStorage = fileStorage;
    }

    public async Task<ClaimSubmissionResult> SubmitClaimAsync(
        ClaimSubmissionInput input,
        CancellationToken cancellationToken = default)
    {
        var fileErrors = await _fileStorage.ValidateTicketUploadAsync(
            input.TicketUpload,
            cancellationToken);

        if (fileErrors.Count > 0)
        {
            return ClaimSubmissionResult.Failure(fileErrors);
        }

        var claimNumber = await GenerateClaimNumberAsync(cancellationToken);
        var compensation = _calculator.Calculate(input.DelayMinutes);
        var storedFile = await _fileStorage.SaveTicketAsync(
            claimNumber,
            input.TicketUpload!,
            cancellationToken);

        var claim = new Claim
        {
            ClaimNumber = claimNumber,
            FullName = input.FullName.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            FlightNumber = input.FlightNumber.Trim().ToUpperInvariant(),
            Airline = input.Airline.Trim(),
            DepartureAirport = input.DepartureAirport.Trim(),
            ArrivalAirport = input.ArrivalAirport.Trim(),
            DelayMinutes = input.DelayMinutes,
            CompensationAmount = compensation.CompensationAmount,
            Status = ClaimStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Documents =
            [
                new ClaimDocument
                {
                    FileName = storedFile.FileName,
                    FilePath = storedFile.FilePath,
                    UploadedAt = DateTime.UtcNow
                }
            ]
        };

        try
        {
            await _claims.AddAsync(claim, cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteTicketAsync(storedFile.FilePath, cancellationToken);
            throw;
        }

        return ClaimSubmissionResult.Success(claimNumber);
    }

    public Task<Claim?> GetClaimByNumberAsync(string claimNumber, CancellationToken cancellationToken = default)
    {
        return _claims.GetByClaimNumberAsync(claimNumber, includeDocuments: false, cancellationToken);
    }

    public Task<Claim?> GetClaimDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return _claims.GetByIdAsync(id, includeDocuments: true, cancellationToken);
    }

    public Task<PagedResult<Claim>> SearchClaimsAsync(
        ClaimSearchCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        return _claims.SearchAsync(criteria, cancellationToken);
    }

    public Task<PublicClaimStats> GetPublicStatsAsync(CancellationToken cancellationToken = default)
    {
        return _claims.GetPublicStatsAsync(cancellationToken);
    }

    public Task<bool> UpdateStatusAsync(
        int id,
        ClaimStatus status,
        CancellationToken cancellationToken = default)
    {
        return _claims.UpdateStatusAsync(id, status, cancellationToken);
    }

    public Task<bool> DeleteClaimAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteClaimCoreAsync(id, cancellationToken);
    }

    private async Task<bool> DeleteClaimCoreAsync(int id, CancellationToken cancellationToken)
    {
        var claim = await _claims.GetByIdAsync(id, includeDocuments: true, cancellationToken);

        if (claim is null)
        {
            return false;
        }

        var documentPaths = claim.Documents
            .Select(document => document.FilePath)
            .Where(filePath => !string.IsNullOrWhiteSpace(filePath))
            .ToList();

        var deleted = await _claims.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return false;
        }

        foreach (var documentPath in documentPaths)
        {
            await _fileStorage.DeleteTicketAsync(documentPath, cancellationToken);
        }

        return true;
    }

    private async Task<string> GenerateClaimNumberAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 10; attempt++)
        {
            var claimNumber = $"FJL-{DateTime.UtcNow:yyyyMMdd}-{RandomNumberGenerator.GetInt32(1000, 9999)}";

            if (!await _claims.ClaimNumberExistsAsync(claimNumber, cancellationToken))
            {
                return claimNumber;
            }
        }

        throw new InvalidOperationException("Unable to generate a unique claim number.");
    }
}
