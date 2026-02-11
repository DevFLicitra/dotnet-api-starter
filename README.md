# dotnet-api-starter

##  Italiano 
ASP.NET Core Web API + SQL Server (Docker) + EF Core + CRUD `Projects` + JWT Auth.

- Validazione con FluentValidation → input non valido = 400 con dettagli.
- Errori in formato ProblemDetails (+ traceId).
- Paginazione: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- Auth JWT: `POST /api/auth/register`, `POST /api/auth/login` (token), `GET /api/auth/me` (protetto).
- Test xUnit: validator + integration tests (Projects + Auth) con `WebApplicationFactory` + EF Core InMemory.

Endpoints:
- `/api/projects`
- `/api/auth/*`

---

##  English 
ASP.NET Core Web API + SQL Server (Docker) + EF Core + `Projects` CRUD + JWT Auth.

- FluentValidation → invalid input = 400 with details.
- ProblemDetails error format (+ traceId).
- Pagination: `GET /api/projects?page=1&pageSize=10` → `PagedResponse`.
- JWT Auth: `POST /api/auth/register`, `POST /api/auth/login` (token), `GET /api/auth/me` (protected).
- xUnit tests: validators + integration tests (Projects + Auth) using `WebApplicationFactory` + EF Core InMemory.

Endpoints:
- `/api/projects`
- `/api/auth/*`
