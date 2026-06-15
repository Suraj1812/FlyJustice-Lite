using FlyJusticeLite.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlyJusticeLite.Services;

public sealed class FileStorageService : IFileStorageService
{
    private static readonly IReadOnlyDictionary<string, string[]> AllowedContentTypes =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = ["application/pdf"],
            [".jpg"] = ["image/jpeg", "image/pjpeg"],
            [".jpeg"] = ["image/jpeg", "image/pjpeg"],
            [".png"] = ["image/png"]
        };

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

    public async Task<IReadOnlyList<string>> ValidateTicketUploadAsync(
        IFormFile? file,
        CancellationToken cancellationToken = default)
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
            return errors;
        }

        if (!AllowedContentTypes.TryGetValue(extension, out var allowedContentTypes) ||
            !allowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            errors.Add("The ticket file type does not match its extension.");
            return errors;
        }

        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var header = new byte[8];
            var bytesRead = await stream.ReadAsync(header.AsMemory(0, header.Length), cancellationToken);

            if (!MatchesFileSignature(extension, header.AsSpan(0, bytesRead)))
            {
                errors.Add("The ticket file content is not a valid PDF, JPG, or PNG.");
            }
        }

        return errors;
    }

    public async Task<StoredFile> SaveTicketAsync(
        string claimNumber,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var originalFileName = Path.GetFileName(file.FileName);
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var relativeDirectory = Path.Combine("claim-documents", claimNumber);
        var absoluteDirectory = Path.Combine(GetPrivateStorageRoot(), claimNumber);

        Directory.CreateDirectory(absoluteDirectory);

        var absolutePath = Path.Combine(absoluteDirectory, storedFileName);
        await using var stream = new FileStream(
            absolutePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);
        await file.CopyToAsync(stream, cancellationToken);

        var storedPath = Path.Combine(relativeDirectory, storedFileName).Replace('\\', '/');
        return new StoredFile(originalFileName, storedPath);
    }

    public Task<StoredFileDownload?> OpenTicketAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        var absolutePath = ResolveStoredPath(filePath);

        if (absolutePath is null || !File.Exists(absolutePath))
        {
            return Task.FromResult<StoredFileDownload?>(null);
        }

        var stream = new FileStream(
            absolutePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 81920,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
        var contentType = GetContentType(Path.GetExtension(absolutePath));

        return Task.FromResult<StoredFileDownload?>(new StoredFileDownload(stream, contentType));
    }

    public Task DeleteTicketAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Task.CompletedTask;
        }

        try
        {
            var absolutePath = ResolveStoredPath(filePath);

            if (absolutePath is null)
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

    private string GetPrivateStorageRoot()
    {
        return Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "App_Data", "claim-documents"));
    }

    private string? ResolveStoredPath(string filePath)
    {
        if (filePath.StartsWith("/uploads/claims/", StringComparison.OrdinalIgnoreCase))
        {
            var webRootPath = string.IsNullOrWhiteSpace(_environment.WebRootPath)
                ? Path.Combine(_environment.ContentRootPath, "wwwroot")
                : _environment.WebRootPath;
            var legacyRoot = Path.GetFullPath(Path.Combine(webRootPath, "uploads", "claims"));
            var legacyRelativePath = filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var legacyPath = Path.GetFullPath(Path.Combine(webRootPath, legacyRelativePath));

            return legacyPath.StartsWith(legacyRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                ? legacyPath
                : null;
        }

        const string privatePrefix = "claim-documents/";

        if (!filePath.StartsWith(privatePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var privateRoot = GetPrivateStorageRoot();
        var relativePath = filePath[privatePrefix.Length..].Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.GetFullPath(Path.Combine(privateRoot, relativePath));

        return absolutePath.StartsWith(privateRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            ? absolutePath
            : null;
    }

    private static bool MatchesFileSignature(string extension, ReadOnlySpan<byte> header)
    {
        return extension switch
        {
            ".pdf" => header.StartsWith("%PDF-"u8),
            ".jpg" or ".jpeg" => header.Length >= 3 &&
                                 header[0] == 0xFF &&
                                 header[1] == 0xD8 &&
                                 header[2] == 0xFF,
            ".png" => header.StartsWith(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            _ => false
        };
    }

    private static string GetContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }

    private static string FormatBytes(long bytes)
    {
        var megabytes = bytes / 1024d / 1024d;
        return $"{megabytes:0.#} MB";
    }
}
