# Student Management System (.NET Core)

This repository contains a monolithic ASP.NET Core Web API for managing students using a repository pattern.

## Structure
- `Controllers/` exposes the API endpoints.
- `Services/` implements business logic.
- `Repositories/` handles data access (in-memory for now).
- `Models/` contains domain entities.
- `Dtos/` contains request/response contracts.

## Running (requires .NET 8 SDK)
Restore packages before the first build:
```bash
dotnet restore src/StudentManagement/StudentManagement.csproj
```

Run the API:
```bash
dotnet run --project src/StudentManagement/StudentManagement.csproj
```

## Authentication (JWT)
Configure the JWT settings in `src/StudentManagement/appsettings.json` before running.

### Auth Endpoints
- `POST /api/auth/signup` (body: `{ "email": "...", "password": "..." }`)
- `POST /api/auth/login` (body: `{ "email": "...", "password": "..." }`)

### Using the token
Include the token in requests to protected endpoints:
```
Authorization: Bearer {token}
```

## API Endpoints
- `GET /api/students`
- `GET /api/students/{id}`
- `POST /api/students`
- `PUT /api/students/{id}`
- `DELETE /api/students/{id}`
