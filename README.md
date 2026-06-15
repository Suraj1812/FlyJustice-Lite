# FlyJustice Lite

FlyJustice Lite is an ASP.NET Core 8 Razor Pages legal-tech app for checking EU flight compensation eligibility, submitting claims with ticket documents, tracking claim status, and browsing passenger-rights guidance inspired by FlyJustice.

## Stack

- ASP.NET Core 8 Razor Pages
- Entity Framework Core
- Microsoft SQL Server
- Repository and service pattern
- Mobile-first CSS with Inter typography
- Dynamic public content service for rights, FAQs, fees, and legal pages
- No login, registration, or authentication flow

## Live Deployment

- App: https://flyjustice-web-production.up.railway.app
- Health check: https://flyjustice-web-production.up.railway.app/health
- Host: Railway
- Database: private Railway SQL Server service (`flyjustice-mssql.railway.internal`)

The live Railway deployment is validated for public content pages, eligibility checks, claim submission with ticket upload, claim tracking, no-auth internal claim management, status updates, and claim deletion.

## Run Locally

1. Update `ConnectionStrings:DefaultConnection` in `appsettings.json` or use user secrets.
2. Start SQL Server.
3. Run:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

In development, the app applies migrations and seeds sample claims on startup.

One quick SQL Server option:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name flyjustice-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

Or run the app and SQL Server together:

```bash
docker compose up --build
```

The containerized app runs at `http://localhost:8080`.

## Useful Routes

- `/` - eligibility checker
- `/Rights` and `/Rights/{slug}` - passenger-rights hub and disruption guides
- `/Services` - claim services and process
- `/Fees` - fee model
- `/Faqs` - passenger-rights FAQs
- `/About` - platform overview
- `/WhyChooseUs` - trust and workflow benefits
- `/Contact` - validated support form
- `/Legal/{slug}` - terms, privacy, GDPR, and imprint pages
- `/Claims/Submit` - submit a claim
- `/Claims/Track` - track a claim by claim number
- `/Admin` - internal no-auth claims list
- `/robots.txt` and `/sitemap.xml` - SEO discovery
- `/health` - app and database health check

## Deployment Notes

This is a server-rendered ASP.NET Core app with MSSQL, so it should be deployed to a server/container host such as Azure App Service, Azure Container Apps, Render, Railway, or a VPS with SQL Server/Azure SQL. GitHub Pages is not suitable for this app because it cannot run Razor Pages or connect server-side to MSSQL.

Set these production values as host secrets or environment variables:

```text
ConnectionStrings__DefaultConnection
Site__PublicBaseUrl
Database__ApplyMigrationsOnStartup=true
Database__SeedSampleData=false
```

For Railway, the web service deploys from the root `Dockerfile`. The SQL Server sidecar runs the official Microsoft SQL Server 2022 image and uses `MSSQL_MEMORY_LIMIT_MB=1536` so the container remains stable on Railway. For long-term production data durability, attach a persistent Railway volume through the Railway dashboard or move the connection string to managed Azure SQL; Railway CLI volume attachment failed during validation.

Authentication is intentionally removed from this build. Do not expose `/Admin` publicly for real operations unless the host, reverse proxy, VPN, or platform access rules restrict it.

## Sample Claim Numbers

The development seeder creates sample claims such as `FJL-20260615-1001`, `FJL-20260615-1002`, and `FJL-20260615-1003`.
