# dotnet-api-starter

[English](#english) | [Italiano](#italiano)

[![CI](https://github.com/DevFLicitra/dotnet-api-starter/actions/workflows/ci.yml/badge.svg)](https://github.com/DevFLicitra/dotnet-api-starter/actions/workflows/ci.yml)

---

## English

A personal starter template I built to have a solid, production-ready base whenever I start a new ASP.NET Core Web API project. It covers the things I always end up needing: auth, security, Docker, CI, tests.

### What's inside

- **ASP.NET Core Web API** (.NET 8)
- **EF Core + SQL Server**
- **JWT Authentication** — register, login, get current user
- **Refresh tokens** with rotation and revoke (no long-lived access tokens)
- **Brute force protection** — account lockout after 5 failed login attempts
- **RBAC** — User and Admin roles, with admin-only endpoints
- **Database seeding** — admin user and demo data on first startup
- **FluentValidation** — request validation out of the box
- **Swagger/OpenAPI** — with Bearer auth support
- **Health checks** — readiness endpoint at `/health`
- **Rate limiting** — fixed window per endpoint
- **Docker end-to-end** — spin up API + SQL Server with a single command
- **GitHub Actions CI** — build and test on every push

---

## Quickstart (Docker) ✅ Recommended

### Requirements

- Docker Desktop

### 1) Create your `.env`

```bash
cp env.example .env
```

Fill it in:

```env
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
JWT_KEY=super-secret-key-change-me-in-production-2024!
```

> Make sure `JWT_KEY` is at least 32 characters or the app will throw on startup.

### 2) Start everything

```bash
docker compose up --build
```

This starts the API and SQL Server together. EF migrations run automatically on startup.

### 3) Open

- Swagger UI: http://localhost:8080/swagger
- Health check: http://localhost:8080/health

---

## Auth endpoints

All endpoints are versioned under `/api/v1/`.

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/auth/register` | Create a new account |
| POST | `/api/v1/auth/login` | Login, returns `accessToken` + `refreshToken` |
| GET | `/api/v1/auth/me` | Get current user info (requires Bearer token) |
| POST | `/api/v1/auth/refresh` | Get new tokens using refresh token |
| POST | `/api/v1/auth/revoke` | Invalidate a refresh token (logout) |

## Admin endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/admin/users` | List all users (Admin role required) |

---

## How auth works

1. Call `/login` → you get a short-lived `accessToken` (1 hour) and a `refreshToken` (7 days)
2. Use the `accessToken` as `Authorization: Bearer <token>` on protected endpoints
3. When it expires, call `/refresh` with the refresh token to get a new pair
4. The old refresh token is immediately invalidated on use (rotation)
5. Call `/revoke` to log out and invalidate the refresh token

---

## Default seed data

On first startup the app seeds:
- Admin user: `admin@admin.com` / `Admin123!`
- 2 demo projects

> Change the admin password immediately in any non-local environment.

---

## Running tests

```bash
dotnet test
```

---

## Project structure

```
Api/
  Controllers/    # AuthController, ProjectsController, AdminController
  Data/           # AppDbContext, DbSeeder
  Domain/         # AppUser, Project, RefreshToken
  Auth/           # JwtSettings
  Migrations/     # EF Core migrations
tests/
  Api.Tests/
  Api.IntegrationTests/
```

---

## Italiano

Un template di partenza che ho costruito per avere una base solida ogni volta che inizio un nuovo progetto API con ASP.NET Core. Copre tutto quello che mi ritrovo sempre a dover aggiungere: auth, sicurezza, Docker, CI, test.

### Cosa include

- **ASP.NET Core Web API** (.NET 8)
- **EF Core + SQL Server**
- **Autenticazione JWT** — register, login, dati utente corrente
- **Refresh token** con rotazione e revoca
- **Protezione brute force** — lockout account dopo 5 tentativi falliti
- **RBAC** — ruoli User e Admin, con endpoint admin-only
- **Seeding del database** — utente admin e dati demo al primo avvio
- **FluentValidation** — validazione delle request
- **Swagger/OpenAPI** — con supporto Bearer auth
- **Health check** — endpoint `/health`
- **Rate limiting** — fixed window per endpoint
- **Docker end-to-end** — API + SQL Server con un comando
- **GitHub Actions CI** — build e test ad ogni push

---

## Avvio rapido (Docker) ✅ Consigliato

### Requisiti

- Docker Desktop

### 1) Crea il `.env`

```bash
cp env.example .env
```

Compilalo:

```env
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
JWT_KEY=super-secret-key-change-me-in-production-2024!
```

> `JWT_KEY` deve essere di almeno 32 caratteri, altrimenti l'app crasha all'avvio.

### 2) Avvia tutto

```bash
docker compose up --build
```

Avvia API e SQL Server insieme. Le migration EF girano in automatico.

### 3) Apri

- Swagger UI: http://localhost:8080/swagger
- Health check: http://localhost:8080/health

---

## Endpoint Auth

Tutti gli endpoint sono versionati sotto `/api/v1/`.

| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| POST | `/api/v1/auth/register` | Crea un nuovo account |
| POST | `/api/v1/auth/login` | Login, restituisce `accessToken` + `refreshToken` |
| GET | `/api/v1/auth/me` | Dati utente corrente (richiede Bearer token) |
| POST | `/api/v1/auth/refresh` | Ottieni nuovi token dal refresh token |
| POST | `/api/v1/auth/revoke` | Invalida un refresh token (logout) |

## Endpoint Admin

| Metodo | Endpoint | Descrizione |
|--------|----------|-------------|
| GET | `/api/v1/admin/users` | Lista tutti gli utenti (richiede ruolo Admin) |

---

## Come funziona l'auth

1. Chiami `/login` → ricevi un `accessToken` (1 ora) e un `refreshToken` (7 giorni)
2. Usi l'`accessToken` come `Authorization: Bearer <token>` sugli endpoint protetti
3. Quando scade, chiami `/refresh` con il refresh token per ottenere una nuova coppia
4. Il vecchio refresh token viene immediatamente invalidato (rotazione)
5. Chiami `/revoke` per fare logout e invalidare il refresh token

---

## Dati di seed

Al primo avvio l'app crea automaticamente:
- Utente admin: `admin@admin.com` / `Admin123!`
- 2 progetti demo

> Cambia la password admin subito in qualsiasi ambiente non locale.

---

## Eseguire i test

```bash
dotnet test
```
