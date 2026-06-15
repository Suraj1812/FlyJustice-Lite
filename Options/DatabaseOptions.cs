namespace FlyJusticeLite.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool ApplyMigrationsOnStartup { get; set; }

    public bool SeedSampleData { get; set; }
}
