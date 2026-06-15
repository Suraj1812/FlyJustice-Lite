using FlyJusticeLite.Models;
using FlyJusticeLite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlyJusticeLite.Pages.Admin;

public sealed class DetailsModel : PageModel
{
    private readonly IClaimService _claims;
    private readonly IFileStorageService _fileStorage;

    public DetailsModel(IClaimService claims, IFileStorageService fileStorage)
    {
        _claims = claims;
        _fileStorage = fileStorage;
    }

    public Claim? Claim { get; private set; }

    [BindProperty]
    public ClaimStatus Status { get; set; }

    public IReadOnlyList<SelectListItem> StatusOptions { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Claim = await _claims.GetClaimDetailsAsync(id, cancellationToken);

        if (Claim is null)
        {
            return NotFound();
        }

        Status = Claim.Status;
        LoadStatusOptions();
        return Page();
    }

    public async Task<IActionResult> OnPostStatusAsync(int id, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(ClaimStatus), Status))
        {
            ModelState.AddModelError(nameof(Status), "Choose a valid status.");
        }

        if (!ModelState.IsValid)
        {
            Claim = await _claims.GetClaimDetailsAsync(id, cancellationToken);
            LoadStatusOptions();
            return Page();
        }

        var updated = await _claims.UpdateStatusAsync(id, Status, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = "Claim status updated.";
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken cancellationToken)
    {
        var deleted = await _claims.DeleteClaimAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["Toast.Type"] = "success";
        TempData["Toast.Message"] = "Claim deleted.";
        return RedirectToPage("/Admin/Index");
    }

    public async Task<IActionResult> OnGetDocumentAsync(
        int id,
        int documentId,
        CancellationToken cancellationToken)
    {
        var claim = await _claims.GetClaimDetailsAsync(id, cancellationToken);
        var document = claim?.Documents.FirstOrDefault(item => item.Id == documentId);

        if (document is null)
        {
            return NotFound();
        }

        var storedFile = await _fileStorage.OpenTicketAsync(document.FilePath, cancellationToken);

        if (storedFile is null)
        {
            return NotFound();
        }

        return new FileStreamResult(storedFile.Content, storedFile.ContentType)
        {
            FileDownloadName = document.FileName,
            EnableRangeProcessing = true
        };
    }

    private void LoadStatusOptions()
    {
        StatusOptions = Enum.GetValues<ClaimStatus>()
            .Select(status => new SelectListItem(status.ToDisplayName(), status.ToString()))
            .ToList();
    }
}
