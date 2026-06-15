using FlyJusticeLite.Models;
using FlyJusticeLite.Services;
using FlyJusticeLite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FlyJusticeLite.Pages.Admin;

public sealed class IndexModel : PageModel
{
    private readonly IClaimService _claims;

    public IndexModel(IClaimService claims)
    {
        _claims = claims;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public PagedResult<Claim> Claims { get; private set; } = PagedResult<Claim>.Empty(1, 10);

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Claims = await _claims.SearchClaimsAsync(
            new ClaimSearchCriteria
            {
                Query = Search,
                PageNumber = PageNumber,
                PageSize = 10
            },
            cancellationToken);
    }
}
