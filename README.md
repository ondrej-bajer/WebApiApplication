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
- Unit and integration test

---

## Architecture Overview

The application follows a layered architecture to ensure separation of concerns and maintainability.

- Controllers handle HTTP requests and delegate business logic to services.

- Services contain business logic and abstract data access via interfaces.

- Data layer is implemented using EF Core with support for both SQL Server and InMemory providers.

- Dependency Injection is used to allow easy switching between data sources and to support testability.

The solution adheres to SOLID principles and keeps responsibilities clearly separated across layers.

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

```Package Manager Console
Update-Database
```

Or CLI:

```Developer PowerSchell
dotnet ef database update
```

---

#### InMemory

In `appsettings.json`:

```json
"DataSource": "InMemory"
```

No database setup is required.

---

### Run

Press `F5` in Visual Studio.

Or CLI:

```Developer PowerSchell
dotnet run --project WebApiApplication
```

---

## Running the Test

Run tests from Visual Studio

Open the solution in Visual Studio

Go to Test → Test Explorer

Click Run All Tests

Or CLI:

```Developer PowerSchell
dotnet test
```
This will:

Build the solution

Execute all tests in the WebApiTests project

Display results in the console