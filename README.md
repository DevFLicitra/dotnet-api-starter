# dotnet-api-starter

[English](#english) | [Italiano](#italiano)

[![CI](https://github.com/DevFLicitra/dotnet-api-starter/actions/workflows/ci.yml/badge.svg)](https://github.com/DevFLicitra/dotnet-api-starter/actions/workflows/ci.yml)

A production-minded ASP.NET Core Web API starter: JWT auth, SQL Server (EF Core), Swagger, health checks, tests, CI, and Docker end-to-end.

---

## English

### What’s inside
- **ASP.NET Core Web API** (.NET)
- **EF Core + SQL Server**
- **JWT Authentication** (Register / Login / Me)
- **Swagger/OpenAPI**
- **Health Checks**
- **Automated tests** + **GitHub Actions CI**
- **Docker end-to-end**: run **API + DB** with one command

---

## Quickstart (Docker) ✅ Recommended

### Requirements
- Docker Desktop

### 1) Create `.env`
Copy from `env.example`:
```bash
cp env.example .env
```

Example `.env`:
```bash
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
JWT_KEY=dev-only-key-change-me-dev-only-key-change-me
```

### 2) Run API + DB
```bash
docker compose up --build
```

### 3) Open
- Swagger UI: http://localhost:8080/swagger
- Health: http://localhost:8080/health

### Auth endpoints (versioned)
- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `GET  /api/v1/auth/me` (Authorization: Bearer `<token>`)

Example (register):
```bash
curl -X POST http://localhost:8080/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"test@example.com\",\"password\":\"Password123!\"}"
```

### Tests
```bash
dotnet test
```

---

## Italiano

### Cosa include
- **ASP.NET Core Web API** (.NET)
- **EF Core + SQL Server**
- **Autenticazione JWT** (Register / Login / Me)
- **Swagger/OpenAPI**
- **Health Checks**
- **Test automatici** + **GitHub Actions CI**
- **Docker end-to-end**: avvia **API + DB** con un solo comando

---

## Avvio rapido (Docker) ✅ Consigliato

### Requisiti
- Docker Desktop

### 1) Crea `.env`
Copia da `env.example`:
```bash
cp env.example .env
```

Esempio `.env`:
```bash
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
JWT_KEY=dev-only-key-change-me-dev-only-key-change-me
```

### 2) Avvia API + DB
```bash
docker compose up --build
```

### 3) Apri
- Swagger UI: http://localhost:8080/swagger
- Health: http://localhost:8080/health

### Endpoint Auth (versionati)
- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `GET  /api/v1/auth/me` (Authorization: Bearer `<token>`)

Esempio (register):
```bash
curl -X POST http://localhost:8080/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"test@example.com\",\"password\":\"Password123!\"}"
```

### Test
```bash
dotnet test
```
