namespace FlyJusticeLite.Options;

public sealed class FileUploadOptions
{
    public const string SectionName = "FileUploads";

    public long MaxBytes { get; set; } = 10 * 1024 * 1024;

    public string[] AllowedExtensions { get; set; } = [".pdf", ".jpg", ".jpeg", ".png"];
}
