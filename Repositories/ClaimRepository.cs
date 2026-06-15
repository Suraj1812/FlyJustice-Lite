using FlyJusticeLite.Data;
using FlyJusticeLite.Models;
using FlyJusticeLite.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlyJusticeLite.Repositories;

public sealed class ClaimRepository : IClaimRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ClaimRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        await _dbContext.Claims.AddAsync(claim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ClaimNumberExistsAsync(string claimNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Claims.AnyAsync(
            claim => claim.ClaimNumber == claimNumber,
            cancellationToken);
    }

    public Task<Claim?> GetByClaimNumberAsync(
        string claimNumber,
        bool includeDocuments = false,
        CancellationToken cancellationToken = default)
    {
        var query = BuildClaimQuery(includeDocuments).AsNoTracking();
        var normalizedClaimNumber = claimNumber.Trim();

        return query.FirstOrDefaultAsync(
            claim => claim.ClaimNumber == normalizedClaimNumber,
            cancellationToken);
    }

    public Task<Claim?> GetByIdAsync(
        int id,
        bool includeDocuments = false,
        CancellationToken cancellationToken = default)
    {
        return BuildClaimQuery(includeDocuments)
            .AsNoTracking()
            .FirstOrDefaultAsync(claim => claim.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Claim>> SearchAsync(
        ClaimSearchCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        var pageSize = Math.Clamp(criteria.PageSize, 5, 50);
        var pageNumber = Math.Max(1, criteria.PageNumber);
        var query = _dbContext.Claims.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Query))
        {
            var search = criteria.Query.Trim();

            query = query.Where(claim =>
                claim.FullName.Contains(search) ||
                claim.Email.Contains(search) ||
                claim.FlightNumber.Contains(search) ||
                claim.ClaimNumber.Contains(search));
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(claim => claim.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Claim>(items, totalItems, pageNumber, pageSize);
    }

    public async Task<PublicClaimStats> GetPublicStatsAsync(CancellationToken cancellationToken = default)
    {
        var stats = await _dbContext.Claims
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => new
            {
                TotalClaims = group.Count(),
                ApprovedClaims = group.Count(claim => claim.Status == ClaimStatus.Approved),
                PotentialCompensation = group.Sum(claim => (decimal?)claim.CompensationAmount) ?? 0,
                AverageDelayMinutes = group.Average(claim => (double?)claim.DelayMinutes) ?? 0
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (stats is null)
        {
            return PublicClaimStats.Empty;
        }

        return new PublicClaimStats(
            stats.TotalClaims,
            stats.ApprovedClaims,
            stats.PotentialCompensation,
            (int)Math.Round(stats.AverageDelayMinutes));
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        ClaimStatus status,
        CancellationToken cancellationToken = default)
    {
        var claim = await _dbContext.Claims.FirstOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);

        if (claim is null)
        {
            return false;
        }

        claim.Status = status;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var claim = await _dbContext.Claims.FirstOrDefaultAsync(
            item => item.Id == id,
            cancellationToken);

        if (claim is null)
        {
            return false;
        }

        _dbContext.Claims.Remove(claim);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Claim> BuildClaimQuery(bool includeDocuments)
    {
        var query = _dbContext.Claims.AsQueryable();

        if (includeDocuments)
        {
            query = query.Include(claim => claim.Documents);
        }

        return query;
    }
}
