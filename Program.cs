using FlyJusticeLite.Data;
using FlyJusticeLite.Options;
using FlyJusticeLite.Repositories;
using FlyJusticeLite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.Configure<FileUploadOptions>(
    builder.Configuration.GetSection(FileUploadOptions.SectionName));
builder.Services.Configure<SiteOptions>(
    builder.Configuration.GetSection(SiteOptions.SectionName));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddSingleton<ICompensationCalculator, CompensationCalculator>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddSingleton<IPublicContentService, PublicContentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapGet("/health", async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
{
    var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

    return canConnect
        ? Results.Ok(new { status = "ok", database = "connected" })
        : Results.Problem("Database connection failed.", statusCode: StatusCodes.Status503ServiceUnavailable);
});
app.MapGet("/robots.txt", (HttpContext context) =>
{
    var baseUrl = GetBaseUrl(context);
    var robots = new StringBuilder()
        .AppendLine("User-agent: *")
        .AppendLine("Allow: /")
        .Append("Sitemap: ")
        .Append(baseUrl)
        .AppendLine("/sitemap.xml")
        .ToString();

    return Results.Text(robots, "text/plain");
});
app.MapGet("/sitemap.xml", (HttpContext context, IPublicContentService content) =>
{
    var baseUrl = GetBaseUrl(context);
    var urls = new List<string>
    {
        "/",
        "/Claims/Submit",
        "/Claims/Track",
        "/Rights",
        "/Services",
        "/Fees",
        "/Faqs",
        "/About",
        "/WhyChooseUs",
        "/Contact"
    };

    urls.AddRange(content.GetRightsPages().Select(page => $"/Rights/{page.Slug}"));
    urls.AddRange(content.GetLegalPages().Select(page => $"/Legal/{page.Slug}"));

    var xml = new StringBuilder()
        .AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""")
        .AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");

    foreach (var url in urls.Distinct(StringComparer.OrdinalIgnoreCase))
    {
        xml.AppendLine("  <url>")
            .Append("    <loc>")
            .Append(baseUrl)
            .Append(url)
            .AppendLine("</loc>")
            .AppendLine("  </url>");
    }

    xml.AppendLine("</urlset>");
    return Results.Text(xml.ToString(), "application/xml");
});

await ApplyDatabaseSetupAsync(app);

await app.RunAsync();

static async Task ApplyDatabaseSetupAsync(WebApplication app)
{
    var databaseOptions = app.Configuration
        .GetSection(DatabaseOptions.SectionName)
        .Get<DatabaseOptions>() ?? new DatabaseOptions();

    if (!databaseOptions.ApplyMigrationsOnStartup && !databaseOptions.SeedSampleData)
    {
        return;
    }

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (databaseOptions.ApplyMigrationsOnStartup)
    {
        await dbContext.Database.MigrateAsync();
    }

    if (databaseOptions.SeedSampleData)
    {
        await DatabaseSeeder.SeedAsync(dbContext);
    }
}

static string GetBaseUrl(HttpContext context)
{
    var configuredBaseUrl = context.RequestServices
        .GetRequiredService<IConfiguration>()
        .GetSection(SiteOptions.SectionName)
        .Get<SiteOptions>()?
        .PublicBaseUrl?
        .TrimEnd('/');

    return string.IsNullOrWhiteSpace(configuredBaseUrl)
        ? $"{context.Request.Scheme}://{context.Request.Host}"
        : configuredBaseUrl;
}
