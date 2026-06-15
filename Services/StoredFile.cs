namespace FlyJusticeLite.Services;

public sealed record StoredFile(string FileName, string FilePath);

public sealed record StoredFileDownload(Stream Content, string ContentType);
