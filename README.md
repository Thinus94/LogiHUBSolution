# Logistics Management System

A multi-tenant logistics management application for managing **customers, shipments, and invoices**.
Each client can securely manage their own operational data using **token-based authentication**.

For system design and architecture details, see **ARCHITECTURE.md**.

---

# Features

* Client authentication with JWT tokens
* Customer management
* Shipment tracking
* Invoice creation and management
* Relationship hierarchy:

```
Client → Customers → Shipments → Invoices
```

* Multi-tenant data isolation (each client only sees their own data)

---

# Technology Stack

Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* AutoMapper
* JWT Authentication

Frontend

* Blazor
* Bootstrap

Database

* SQL Server (via Entity Framework Core)

---

# Project Structure

```
/Server        → ASP.NET Web API
/Client        → Blazor Frontend
/Shared        → Shared DTOs and Models
ARCHITECTURE.md
README.md
```

---

# Setup Instructions

## 1. Clone the repository

```
git clone <repository-url>
cd <repository-folder>
```

---

## 2. Configure the database

Update the connection string in:

```
appsettings.json
```

Example:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LogisticsDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 3. Apply database migrations

Run:

```
dotnet ef database update
```

---

## 4. Run the application

From the project root:

```
dotnet run
```

Or run the **Server project** from Visual Studio.

---

## 5. Access the application

Backend API:

```
https://localhost:<port>/api
```

Frontend:

```
https://localhost:<port>
```

---

# Authentication

The system uses **JWT token authentication**.

Login returns a token which must be included in requests:

```
Authorization: Bearer <token>
```

The token determines the **ClientId**, ensuring each client only accesses their own data.

---

# Documentation

For system architecture and model relationships see:

```
ARCHITECTURE.md
```

---

# Future Improvements

Planned extensions include:

* Payment tracking
* Shipment status automation
* Reporting and analytics
* Advanced filtering and dashboards
