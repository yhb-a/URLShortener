https://roadmap.sh/projects/url-shortening-service

# URLShortener

Lightweight URL shortener built with ASP.NET Core targeting .NET 10. Uses Entity Framework Core + PostgreSQL and exposes a simple HTTP API with OpenAPI available in Development.

## Overview
`URLShortener` provides:
- Shorten long URLs to compact short codes and redirect requests to original URLs.
- Persistent storage via EF Core + Npgsql (PostgreSQL).
- Clear separation of concerns: repository (`IURLRepository`), service (`IURLService`), and a singleton `IGlobalCounterService` for short-code generation.
- A hosted service (`CounterInitializerService`) that initializes the global counter on startup.

## Prerequisites
- .NET 10 SDK installed
- PostgreSQL server accessible (local or remote)
- Optional: Visual Studio 2026 for editing and debugging

## Configuration
Set your PostgreSQL connection string under `ConnectionStrings:DefaultConnection` in `appsettings.json`, or supply it via environment variable.

Example `appsettings.json` snippet:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Username=your_user;Password=your_pass;Database=urlshortener_db"
  }
}
```

## Setup (migrations already present)
You already have migrations in the project. The setup below assumes migration files exist under `URLShortener/Migrations/`. The only required DB step is to apply (update) the database.

From the repository root (recommended):

1. Restore and build

2. Ensure EF Core tooling is available (one-time if not already installed)

3. Apply migrations by executing: `dotnet ef database update`

4. Start the app

## Notes & Troubleshooting
- If `dotnet ef` is not found, run `dotnet tool restore` or install `dotnet-ef` globally.
- If the update fails, verify `ConnectionStrings:DefaultConnection` points to a reachable PostgreSQL instance and credentials are correct.
- If you change models later, create a new migration (`dotnet ef migrations add <Name>`) and then run `dotnet ef database update`.
- If no migrations present run `dotnet ef migrations add InitialCreate` first to create the initial migration and then run `dotnet ef database update` to apply it.
