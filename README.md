# dotnet-api-starter

## 🇮🇹 Italiano (breve)
ASP.NET Core Web API + SQL Server (Docker) + EF Core + CRUD `Projects`.

- Validazione con FluentValidation → input non valido = 400 con dettagli.
- Errori in formato ProblemDetails (+ traceId).
- Paginazione: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- Test xUnit: validator + integration test `/api/projects` (POST + GET) con `WebApplicationFactory` + EF Core InMemory.

Endpoint: `/api/projects`

---

## English (short)
ASP.NET Core Web API + SQL Server (Docker) + EF Core + `Projects` CRUD.

- FluentValidation → invalid input = 400 with details.
- ProblemDetails error format (+ traceId).
- Pagination: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- xUnit tests: validators + `/api/projects` integration test (POST + GET) using `WebApplicationFactory` + EF Core InMemory.

Endpoint: `/api/projects`