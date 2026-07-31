# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from the repo root. The solution file is at `BookyApp/BookyApp.sln`.

```bash
# Build
dotnet build BookyApp/BookyApp.sln

# Run the API
dotnet run --project BookyApp/BookyApp

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project BookyApp/Infra --startup-project BookyApp/BookyApp

# Apply migrations to the database
dotnet ef database update --project BookyApp/Infra --startup-project BookyApp/BookyApp
```

Swagger UI is available at `/swagger` when running in Development mode.

## Architecture

Clean Architecture with four projects:

| Project | Role |
|---|---|
| `Domain` | Entities only — no dependencies |
| `Application` | Interfaces (`Contracts/`) and DTOs — no implementations |
| `Infra` | EF Core, service implementations, migrations |
| `BookyApp` | ASP.NET Core Web API host — controllers, middleware |

`Infrastructure/` exists in the solution but is an empty stub — ignore it, the real infra is in `Infra/`.

### Dependency flow
`BookyApp` → `Infra` → `Application` → `Domain`

### Key patterns

**Generic repository** — `IBaseRepository<T>` (Application) / `BaseRepository<T>` (Infra) provides `GetAsync`, `GetMany`, `GetManyAsync`, `FindById`, `Add`, `Delete`, `UpdateAsync`, `SaveChangesAsync`. Controllers inject the service interfaces; repositories are only used inside services.

**Session / audit** — `SessionFilter` (action filter) runs before every controller action and populates the scoped `Session` object with `UserId`, `Email`, `FullName`, `Role` from JWT claims. `AppDbContext.SaveChangesAsync` reads `Session.UserId` to auto-fill `EntityBase.CreatedBy` / `CreatedDate` / `UpdatedDate`. All entities that need audit fields must inherit `EntityBase`.

**Service registration** — everything is wired in `Infra/Helper/Extensions/InfrastructureRegistration.cs` via three extension methods called from `Program.cs`: `AddInfra` (DbContext + Identity + JWT), `AddFluentEmail`, `AddMapster`, `AddApplicationRegitrations`.

**JWT auth** — Bearer token, validated in `InfrastructureRegistration.AddIdentity`. Key, Issuer, and Audience come from `appsettings.json` `JWT` section. Token generation is in `Infra/Services/TokenService.cs`.

**CORS** — `AllowAnyOrigin / AllowAnyHeader / AllowAnyMethod` (default policy, open for development).

**Static files** — book cover images are served from `BookyApp/wwwroot/Books/` via `UseStaticFiles`.

### Feed endpoint

`GET /api/Quotation/GetFeed?Pagenation.pageNumber=1&Pagenation.pageSize=20`

Returns all quotations sorted so that quotations from books whose genres match the current user's interests appear first, then the rest ordered by `CreatedDate` descending. Sorting is done via `OrderByDescending(q => q.Book.BookGenres.Any(bg => userInterestIds.Contains(bg.GenrId)))` — EF Core translates this to a SQL `CASE` expression. Requires the user to be authenticated (Session.UserId populated by `SessionFilter`).

### Domain model summary
- `ApplicationUser` extends `IdentityUser` (ASP.NET Core Identity)
- `Book` ← genres via `BookGenres` join, favorites via `FavoriteUserBooks`
- `Quotation` ← `QuotationLike`, `QuotationShare`, `ReQuote`, `Comment`
- `Genres` / `UserInterest` — user genre preferences
- Seeded data: genre list + Admin role + Admin user (email: `admin@admin.com`)

### Configuration
Local dev uses SQL Server: `Server=.;Database=books9;Trusted_Connection=True;TrustServerCertificate=True;`
Email uses Gmail SMTP; credentials are in `appsettings.json` (not secrets-managed in dev).
