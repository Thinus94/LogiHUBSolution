# Logistics Management System – Architecture Documentation

---

# 1. Project Purpose:

The Logistics Management System is a multi-tenant web application designed to help logistics companies manage their operational data in a structured way.

The application allows a client organization to manage:
 * Their customers
 * The shipments for those customers
 * The invoices associated with shipments

Each client only sees their own data through token-based authentication, ensuring data isolation between tenants.

The goal of the system is to provide a simple operational overview of logistics flows and financial transactions, while maintaining clear relationships between customers, shipments, and invoices.
		
---

# 2. Core Business Concepts:

The application models a logistics workflow where financial transactions are tied to shipment operations.

## Main entities:

 * Client
   Represents a company using the system.

 * Customer
   Represents a customer of the logistics company.

 * Shipment
   Represents a logistics shipment between an origin and destination.

 * Invoice
   Represents a billing document related to a shipment.

---

# 3. Model Hierarchy:

The data model follows a hierarchical relationship structure.

```
Client
	└── Customers
			└── Shipments
					└── Invoices
```

Relationship explanation:

## Client:

A Client represents a tenant of the system.

	Each client has:
	 * Login credentials
	 * Access to their own dataset only
	 * Token-based authentication
	
	Relationship:
	 * Client (1) → (Many) Customers

## Customer:
	
A Customer belongs to a client and represents a business partner receiving logistics services.

A customer can have multiple shipments.

	Relationship:
	 * Customer (1) → (Many) Shipments

## Shipment:

A Shipment represents a logistics operation, with details about origin, destination, and status.

A shipment can have multiple invoices.

	Relationship:
	* Shipment (1) → (Many) Invoices

## Invoice:

	An Invoice represents a billing document for a shipment, containing financial details.
	
	Relationship:
	 * Shipment (1) → (Many) Invoices
	 * Customer (1) → (Many) Invoices

---

# 4. Authentication and Security:

The system uses token-based authentication (JWT).

	Authentication flow:
	 1. Client logs in using credentials
	 2. Backend validates the login
	 3. Backend issues a JWT token
	 4. The token is stored on the frontend
	 5. The token is included in API requests

``` 
 Header Example:
 Authorization: Bearer <token>
```

Multi-tenant data isolation
 * Each request extracts the ClientId from the token.
 * All database queries are filtered by: ClientId

```
Example concept:
	Invoices
	.Where(i => i.Customer.ClientId == clientId)
```

This ensures:
 * Clients cannot access each other's data
 * All data is automatically scoped to the authenticated client

---

# 5. Application Architecture:

The system follows a standard modern web application architecture.

	Frontend (Blazor)
			↓
	API Controllers (.NET)
			↓
	Services
			↓
	Entity Framework Core
			↓
	Database

## Layers:

Frontend:

	Responsible for UI and user interaction.

	Features include:
	 * Invoice list
	 * Shipment list
	 * Customer list
	 * Create/Edit forms
	 * Navigation between entities

	Technology:
	* Blazor

API Layer:

	API controllers expose endpoints used by the frontend.

	Example endpoints:
	 - GET /api/customers
	 - GET /api/shipments
	 - GET /api/invoices
	 - POST /api/invoices

	Responsibilities:
	 * Validate requests
	 * Authorize access
	 * Call services
	 * Return DTOs

Service Layer

	The service layer contains application logic.

	Responsibilities include:
	 * Data transformations
	 * API calls from frontend
	 * Business logic
	 * Status calculations (e.g. overdue invoices)

	Example:
	 * InvoiceService
	 * ShipmentService
	 * CustomerService

Data Layer:

	The data layer uses Entity Framework Core.

	Responsibilities:
	 * Database access
	 * Entity relationships
	 * Query filtering
	 * Includes for related data
	
	Example:
		_context.Invoices
		    .Include(i => i.Customer)
		    .Include(i => i.Shipment)

---
	
# 6. Technologies Used:
		
Backend:
 * .NET 8
 * ASP.NET Core Web API
 * Entity Framework Core
 * AutoMapper
 * JWT Authentication

Frontend:
 * Blazor WASM
 * Bootstrap
 * Component-based UI

Database
 * SQL Database
 * Managed through Entity Framework Core

---

# 7. Navigation Structure:

The UI allows users to navigate through the entity hierarchy.

Example flows:
```
	Customer → Shipments:

		Customer Details
		      ↓
		View Shipments

	Shipment → Invoices:

		Shipment Details
		      ↓
		Create Invoice
		      ↓
		View Shipment Invoices
```
Invoice Details:
 * Invoices can link back to:
	* Customer
	* Shipment
	* This enables quick navigation through the operational hierarchy.

---

# 8. Application Goals:

The architecture is designed to:
 * Maintain clear entity relationships
 * Support multi-tenant clients
 * Allow easy extension of the domain
 * Keep frontend and backend cleanly separated
 * Support future features such as:
	* payment tracking
	* reporting
	* shipment tracking
	* customer analytics