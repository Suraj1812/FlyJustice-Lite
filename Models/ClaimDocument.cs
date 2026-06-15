namespace FlyJusticeLite.Models;

public sealed class ClaimDocument
{
    public int Id { get; set; }

    public int ClaimId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Claim Claim { get; set; } = null!;
}
