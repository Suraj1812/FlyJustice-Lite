using FlyJusticeLite.Models;
using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public interface IClaimService
{
    Task<ClaimSubmissionResult> SubmitClaimAsync(ClaimSubmissionInput input, CancellationToken cancellationToken = default);

    Task<Claim?> GetClaimByNumberAsync(string claimNumber, CancellationToken cancellationToken = default);

    Task<Claim?> GetClaimDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedResult<Claim>> SearchClaimsAsync(ClaimSearchCriteria criteria, CancellationToken cancellationToken = default);

    Task<PublicClaimStats> GetPublicStatsAsync(CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(int id, ClaimStatus status, CancellationToken cancellationToken = default);

    Task<bool> DeleteClaimAsync(int id, CancellationToken cancellationToken = default);
}
