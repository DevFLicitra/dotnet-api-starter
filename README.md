# dotnet-api-starter

## 🇮🇹 Italiano 
API backend in **ASP.NET Core** con **SQL Server in Docker** e **EF Core**.  
Include un CRUD completo per `Projects` (visibile in Swagger) e una pipeline **CI** (build + test) su GitHub Actions.

**Run rapido**
- DB: crea `.env` da `env.example` → `docker compose up -d`
- Secrets: `dotnet user-secrets init --project Api/Api.csproj` + set `ConnectionStrings:Default`
- Migrazioni: `dotnet ef database update --project Api/Api.csproj --startup-project Api/Api.csproj`
- Avvio: `dotnet run --project Api/Api.csproj` → Swagger `http://localhost:5236/swagger`

Endpoint: `/api/projects`

---

## English 
Backend **ASP.NET Core Web API** with **SQL Server (Docker)** and **EF Core**.  
Includes full `Projects` CRUD (Swagger-ready) and **CI** (build + test) via GitHub Actions.

**Quick run**
- DB: create `.env` from `env.example` → `docker compose up -d`
- Secrets: init + set `ConnectionStrings:Default`
- Migrations: `dotnet ef database update --project Api/Api.csproj --startup-project Api/Api.csproj`
- Run: `dotnet run --project Api/Api.csproj` → Swagger `http://localhost:5236/swagger`

Endpoint: `/api/projects`






