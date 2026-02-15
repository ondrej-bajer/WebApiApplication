# WebApiApplication – Product REST API

## Overview

This project is a REST API built with **.NET 8 (LTS)** and **ASP.NET Core**.

It provides product endpoints with:

- API versioning (v1 and v2)
- Swagger documentation
- Partial update (PATCH description)
- Database-level pagination (v2)
- Support for MSSQL (LocalDB) and InMemory data source
- EF Core migrations and seed data

---

## Technologies

- .NET 8 (LTS)
- ASP.NET Core Web API
- EF Core
- SQL Server LocalDB
- Asp.Versioning.Mvc
- Swashbuckle (Swagger)
- xUnit

---

## API Endpoints

### v1

```http
GET    /api/v1/products
GET    /api/v1/products/{id}
PATCH  /api/v1/products/{id}/description
```

### v2

```http
GET /api/v2/products?page=1&pageSize=10
```

Supports pagination.

Default values:

- `page = 1`
- `pageSize = 10` (maximum 100)

---

## Swagger

Swagger UI is available at:

```text
https://localhost:{port}/swagger
```

Documentation is separated by API version (v1 and v2).

---

## Running the Application

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (if using SQL mode)
- Set `WebApiApplication` as Startup Project in Visual Studio

---

### Choose Data Source

#### SQL Server (LocalDB)

In `appsettings.json`:

```json
"DataSource": "Sql"
```

Apply migrations:

Package Manager Console:

```powershell
Update-Database
```

Or CLI:

```bash
dotnet ef database update
```

---

#### InMemory

In `appsettings.json`:

```json
"DataSource": "InMemory"
```

---

### Run

Press `F5` in Visual Studio.