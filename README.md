##  Italiano 
ASP.NET Core Web API + SQL Server (Docker) + EF Core + CRUD `Projects`.

- Validazione con FluentValidation → input non valido = 400 con dettagli.
- Errori in formato ProblemDetails (+ traceId).
- Paginazione: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- Test xUnit per i validator.

Endpoint: `/api/projects`

## English 
ASP.NET Core Web API + SQL Server (Docker) + EF Core + `Projects` CRUD.

- FluentValidation → invalid input = 400 with details.
- ProblemDetails error format (+ traceId).
- Pagination: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- xUnit tests for validators.

Endpoint: `/api/projects`