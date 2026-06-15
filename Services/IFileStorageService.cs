using Microsoft.AspNetCore.Http;

namespace FlyJusticeLite.Services;

public interface IFileStorageService
{
    Task<IReadOnlyList<string>> ValidateTicketUploadAsync(
        IFormFile? file,
        CancellationToken cancellationToken = default);

    Task<StoredFile> SaveTicketAsync(string claimNumber, IFormFile file, CancellationToken cancellationToken = default);

    Task<StoredFileDownload?> OpenTicketAsync(string filePath, CancellationToken cancellationToken = default);

    Task DeleteTicketAsync(string filePath, CancellationToken cancellationToken = default);
}
