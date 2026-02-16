# dotnet-api-starter

## Descrizione

API di esempio in .NET 8 con funzionalità base: registrazione/login con JWT, gestione progetti, validazione e rate limiting. Utile per capire come strutturare un'API REST sicura e scalabile.

## Endpoint principali

- **POST** `/api/auth/register`: Registra un nuovo utente
- **POST** `/api/auth/login`: Autenticazione e generazione JWT
- **GET** `/api/auth/me`: Dettagli utente autenticato (solo con JWT valido)

- **GET** `/api/projects`: Lista progetti con paginazione
- **POST** `/api/projects`: Crea un nuovo progetto
- **GET** `/api/projects/{id}`: Dettaglio progetto
- **PUT** `/api/projects/{id}`: Modifica progetto

## Rate Limiting

Limite di 60 richieste ogni 1 minuto. Superato il limite, l'API restituisce errore `429 Too Many Requests`.

## Test

I test sono separati in **Unit Test** e **Integration Test** per garantire il corretto funzionamento dell'API.

Per eseguire i test:

```bash
dotnet test