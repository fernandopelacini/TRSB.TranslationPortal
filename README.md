📘 TRSB Translation Portal — README.md
📝 Overview
The TRSB Translation Portal is a multi‑tenant ASP.NET Core MVC + API application designed for managing translation requests across two predefined organizations:

Alpha Traductions

Bêta Légal

Users can register, log in, submit translation requests, and view completed translations.
The system enforces strict organization isolation, ensuring users can only access their own organization’s data.

The project demonstrates:

- Clean architecture
- MediatR (CQRS)
- Strategy Pattern
- FluentValidation
- JWT + Cookie authentication
- EF Core + SQL Server
- MVC + API separation

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
🔐 Security

🔑 Authentication & Authorization
The application uses:

- JWT tokens generated at login
- Tokens stored in HTTP-only cookies
- Cookies injected into Authorization: Bearer <token> header
- HTTPS redirection
- Claims-based authorization

[Authorize] used for protected pages

🏢 Organization Isolation
A core requirement:

Users must never access translation requests from another organization.

This is enforced in:

- Query handlers
```csharp
///GetTranslationRequestByIdHandler
if (entity.OrganizationId != request.OrganizationId)
    return null;
...
```

- API controllers
- MVC controllers

All queries include OrganizationId, and handlers return null if the request does not belong to the user’s organization.

🔒Secure Password Storage
Passwords are never stored in plain text.
The system uses:
- A unique salt per user
- A secure hash
- A dedicated verification method

```csharp
CreatePasswordHash(password, out hash, out salt);
VerifyPasswordHash(password, hash, salt);
```
This protects user credentials even if the database were compromised.

🔧 Strategy Pattern (Translation Engines)
The system includes two translation engines:

ReverseEngine → reverses text

UppercaseEngine → converts text to uppercase

~~A TranslationEngineSelector chooses the engine based on the user’s organization:~~

~~Alpha → Reverse~~

~~Beta → Uppercase~~

A TranslationEngineSelector chooses a engine randomly:

Reverse => Original text is reversed.
UpperCase => Original text is converted to uppercase.
Rotate => Text is replace using an old chiper technique.


Used in CompleteTranslationRequestHandler.

✔️ Features

User:
- Register (auto‑assigned to Alpha or Beta)
- Login (email OR username)
- Logout

Translation Requests
- Submit new request
- View own requests (“Mes demandes”)
- View translation details
- Complete translation (API)
- Organization isolation

Processing Behavior (Simplified for Prototype)
The test requires three statuses: Soumise, En traitement, Complétée.
For simplicity, the translation is processed synchronously when the user clicks Traiter.
The request briefly enters En traitement internally, then immediately becomes Complétée.
In a real production system, this step would be handled by a background worker or queue.
This simplification is intentional and aligns with the test’s guideline to keep things simple.

- Patterns
- Strategy Pattern
- Mediator Pattern (MediatR)
- Repository Pattern
- DTOs
- FluentValidation

🧭 Input Validation (FluentValidation)
All user‑provided data is validated before processing:

- Email format
- Username length
- Full name required
- Password policy (length + special characters, configurable via appsettings)

This prevents:
- Malformed input
- Weak passwords
- Accidental or malicious data injection

🧱 Clean Architecture = Security by Design
The project follows a strict layered architecture:

- Domain → pure business entities
- Application → handlers, rules, validators
- Infrastructure → EF Core, repositories
- Web → controllers, authentication, routing

This separation:
- Reduces side effects
- Prevents accidental data exposure
- Makes isolation rules easy to enforce
- Keeps authentication concerns out of business logic

🧪 Security‑Relevant Tests
Automated tests cover:

- User registration
- Login
- Password validation
- Translation request creation
- Translation processing
- Organization isolation (404 behavior)
- Engine selection logic
- These tests ensure that core protections remain intact as the code evolves.

Database Setup
1. Create database
```sql
CREATE DATABASE TranslationPortal;
```
2. Configure connection string
04-Web/appsettings.json:
```json
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

- Register a user → auto‑assigned to Alpha/Beta

- Login with username OR email

- Submit a translation request

- View “Mes demandes”

- Complete a request (API)

- Verify translation engine selection

- Verify organization isolation

- Verify HTTPS redirection

Bonus:  
We included a small demo endpoint that intentionally returns 403 Forbidden  
to showcase how authorization failures are handled.
