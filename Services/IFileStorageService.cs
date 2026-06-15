using Microsoft.AspNetCore.Http;

namespace FlyJusticeLite.Services;

public interface IFileStorageService
{
    IReadOnlyList<string> ValidateTicketUpload(IFormFile? file);

    Task<StoredFile> SaveTicketAsync(string claimNumber, IFormFile file, CancellationToken cancellationToken = default);

    Task DeleteTicketAsync(string filePath, CancellationToken cancellationToken = default);
}
