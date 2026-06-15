using FlyJusticeLite.Models;
using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Repositories;

public interface IClaimRepository
{
    Task AddAsync(Claim claim, CancellationToken cancellationToken = default);

    Task<bool> ClaimNumberExistsAsync(string claimNumber, CancellationToken cancellationToken = default);

    Task<Claim?> GetByClaimNumberAsync(string claimNumber, bool includeDocuments = false, CancellationToken cancellationToken = default);

    Task<Claim?> GetByIdAsync(int id, bool includeDocuments = false, CancellationToken cancellationToken = default);

    Task<PagedResult<Claim>> SearchAsync(ClaimSearchCriteria criteria, CancellationToken cancellationToken = default);

    Task<PublicClaimStats> GetPublicStatsAsync(CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(int id, ClaimStatus status, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
