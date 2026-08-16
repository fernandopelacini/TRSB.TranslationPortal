📘 TRSB Translation Portal — README.md
📝 Overview
The TRSB Translation Portal is a multi‑tenant ASP.NET Core MVC + API application designed for managing translation requests across two predefined organizations:

Alpha Traductions

Bêta Légal

Users can register, log in, submit translation requests, and view completed translations.
The system enforces strict organization isolation, ensuring users can only access their own organization’s data.

The project demonstrates:

Clean architecture

MediatR (CQRS)

Strategy Pattern

FluentValidation

JWT + Cookie authentication

EF Core + SQL Server

MVC + API separation

🏗️ Architecture
```Code
TRSB.TranslationPortal
│
├── 01-Domain
│   └── Entities (User, TranslationRequest, Organization)
│
├── 02-Application
│   ├── Commands (Register, Login, CreateRequest, CompleteRequest)
│   ├── Queries (GetUserRequests, GetRequestById)
│   ├── Interfaces (Repositories, TranslationEngine)
│   ├── Services (JWT, Strategy Selector, Engines)
│   └── Validators (FluentValidation)
│
├── 03-Infrastructure
│   ├── AppDbContext (EF Core)
│   ├── Repositories (UserRepository, TranslationRequestRepository)
│   └── Migrations
│
└── 04-Web
    ├── MVC Controllers (Account, TranslationPage)
    ├── API Controllers (TranslationController)
    ├── Views (Login, Register, MyRequests, Create, Details)
    └── Program.cs (DI, Auth, Routing)
```
🔐 Authentication
The application uses:

JWT tokens generated at login

Tokens stored in HTTP-only cookies

Cookies injected into Authorization: Bearer <token> header

[Authorize] used for protected pages

🏢 Organization Isolation
A core requirement:

Users must never access translation requests from another organization.

This is enforced in:

Query handlers

API controllers

MVC controllers

All queries include OrganizationId, and handlers return null if the request does not belong to the user’s organization.

🔧 Strategy Pattern (Translation Engines)
The system includes two translation engines:

ReverseEngine → reverses text

UppercaseEngine → converts text to uppercase

A TranslationEngineSelector chooses the engine based on the user’s organization:

Alpha → Reverse

Beta → Uppercase

Used in CompleteTranslationRequestHandler.

✔️ Features
User
Register (auto‑assigned to Alpha or Beta)

Login (email OR username)

Logout

Translation Requests
Submit new request

View own requests (“Mes demandes”)

View translation details

Complete translation (API)

Organization isolation

Security
HTTPS redirection

JWT + Cookies

Claims-based authorization

Patterns
Strategy Pattern

Mediator Pattern (MediatR)

Repository Pattern

DTOs

FluentValidation

Database Setup
1. Create database
```sql
CREATE DATABASE TranslationPortal;
```
2. Configure connection string
04-Web/appsettings.json:
```json
json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TranslationPortal;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
3. Apply migrations
From 04-Web:

```bash
dotnet ef database update
```
🚀 Running the Application
1. Build
```bash
dotnet build
```
2. Run
```bash
dotnet run --project 04-Web
```
App starts at:

Code
https://localhost:5001
http://localhost:5000
HTTPS redirection is enabled.

📦 Publishing for Production
```bash
dotnet publish -c Release -o ./publish
```
Deploy the publish folder to:

IIS

Azure App Service

Docker container

Any ASP.NET Core hosting environment

Ensure environment variable:

Code
ASPNETCORE_ENVIRONMENT=Production
🧪 Smoke Test
After deployment:

Register a user → auto‑assigned to Alpha/Beta

Login with username OR email

Submit a translation request

View “Mes demandes”

Complete a request (API)

Verify translation engine selection

Verify organization isolation

Verify HTTPS redirection
