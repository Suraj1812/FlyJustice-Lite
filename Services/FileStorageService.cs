using FlyJusticeLite.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlyJusticeLite.Services;

public sealed class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly FileUploadOptions _options;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(
        IWebHostEnvironment environment,
        IOptions<FileUploadOptions> options,
        ILogger<FileStorageService> logger)
    {
        _environment = environment;
        _options = options.Value;
        _logger = logger;
    }

    public IReadOnlyList<string> ValidateTicketUpload(IFormFile? file)
    {
        var errors = new List<string>();

        if (file is null)
        {
            errors.Add("Upload a ticket document as a PDF, JPG, or PNG file.");
            return errors;
        }

        if (file.Length == 0)
        {
            errors.Add("The uploaded ticket file is empty.");
        }

        if (file.Length > _options.MaxBytes)
        {
            errors.Add($"The ticket file must be smaller than {FormatBytes(_options.MaxBytes)}.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = _options.AllowedExtensions.Select(item => item.ToLowerInvariant()).ToHashSet();

        if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension))
        {
            errors.Add("Only PDF, JPG, and PNG ticket files are allowed.");
        }

        return errors;
    }

    public async Task<StoredFile> SaveTicketAsync(
        string claimNumber,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var originalFileName = Path.GetFileName(file.FileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var relativeDirectory = Path.Combine("uploads", "claims", claimNumber);
        var absoluteDirectory = Path.Combine(webRootPath, relativeDirectory);

        Directory.CreateDirectory(absoluteDirectory);

        var absolutePath = Path.Combine(absoluteDirectory, storedFileName);
        await using var stream = File.Create(absolutePath);
        await file.CopyToAsync(stream, cancellationToken);

        var browserPath = "/" + Path.Combine(relativeDirectory, storedFileName).Replace('\\', '/');
        return new StoredFile(originalFileName, browserPath);
    }

    public Task DeleteTicketAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath) ||
            !filePath.StartsWith("/uploads/claims/", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        try
        {
            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }

            var uploadsRoot = Path.GetFullPath(Path.Combine(webRootPath, "uploads", "claims"));
            var relativePath = filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var absolutePath = Path.GetFullPath(Path.Combine(webRootPath, relativePath));

            if (!absolutePath.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }

            var directory = Path.GetDirectoryName(absolutePath);

            if (!string.IsNullOrWhiteSpace(directory) &&
                Directory.Exists(directory) &&
                !Directory.EnumerateFileSystemEntries(directory).Any())
            {
                Directory.Delete(directory);
            }
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            _logger.LogWarning(exception, "Unable to delete uploaded ticket file {FilePath}.", filePath);
        }

        return Task.CompletedTask;
    }

    private static string FormatBytes(long bytes)
    {
        var megabytes = bytes / 1024d / 1024d;
        return $"{megabytes:0.#} MB";
    }
}
