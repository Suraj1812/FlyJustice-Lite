# FlyJustice Lite

FlyJustice Lite is an ASP.NET Core 8 Razor Pages app for checking EU flight delay compensation eligibility, submitting claims with ticket documents, tracking claim status, and managing claims from an admin area.

## Stack

- ASP.NET Core 8 Razor Pages
- Entity Framework Core
- Microsoft SQL Server
- Repository and service pattern
- Mobile-first CSS with Inter typography

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
- `/Claims/Submit` - submit a claim
- `/Claims/Track` - track a claim by claim number
- `/Admin` - admin claims list

## Development Admin Login

- Username: `admin`
- Password: `ChangeMe!12345`

Set `Admin:Username` and `Admin:Password` through secrets or environment variables before deploying.

## Deployment Notes

This is a server-rendered ASP.NET Core app with MSSQL, so it should be deployed to a server/container host such as Azure App Service, Azure Container Apps, Render, Railway, or a VPS with SQL Server/Azure SQL. GitHub Pages is not suitable for this app because it cannot run Razor Pages or connect server-side to MSSQL.

Set these production values as host secrets or environment variables:

```text
ConnectionStrings__DefaultConnection
Admin__Username
Admin__Password
Site__PublicBaseUrl
Database__ApplyMigrationsOnStartup=true
Database__SeedSampleData=false
```

## Sample Claim Numbers

The development seeder creates sample claims such as `FJL-20260615-1001`, `FJL-20260615-1002`, and `FJL-20260615-1003`.
