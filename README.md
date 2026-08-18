📘 TRSB Translation Portal — README.md

📝 Overview

The TRSB Translation Portal is a multi‑tenant **ASP.NET Core 9 MVC + API** application designed for managing translation requests across two predefined organizations:

- **Alpha Traductions**

- **Bêta Légal**

Users can:

- Register
- Log in
- Submit translation requests
- View their organization’s requests
- View completed translations
  
The system enforces strict **organization isolation**, ensuring users can only access data belonging to their own organization.

This project demonstrates:

- Clean architecture
- CQRS with MediatR
- Strategy Pattern
- FluentValidation
- JWT + Cookie authentication
- EF Core 9 + SQL Server 2025
- MVC + API separation
- Automated tests

🚀 How to Run the Application (Quick Start)

Prerequisites:
- .NET 9 SDK
- SQL Server 2025 (or SQL Server 2019/2022 — all compatible)
- Windows, macOS, or Linux

⤵️ Clone the repository
```bash
git clone https://github.com/your-repo/trsb-translation-portal.git
cd trsb-translation-portal
```

🗄️ Database Setup

EF Core migrations automatically create the database if it does not exist.

To initialize the database:

Configure connection string
Edit 04.Web/appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TranslationPortal;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
Apply EF Core migrations
```bash
dotnet ef database update --project 03.Infrastructure --startup-project 04.Web
```

Update configuration for JWT Tokens 
Edit 04.Web/appsettings.json (In my case I've set everything to user secrets, in a real scenario Azure Vault Key or similar should be used to protect keys)

```json
"Jwt": {
  "Key": "<JWT KEY>",
  "Issuer": "<JWT ISSUER>",
  "Audience": "<JWT AUDIENCE>",
  "ExpiresMinutes": 60
},
```

Run the application
```bash
dotnet run --project 04-Web
```
The app starts at:

- https://localhost:7296
- http://localhost:5296

HTTPS redirection is enabled.

🧰 Technologies Used

| Component | Version | Notes |
| --------- | ------- | ----- |
| .NET	    | 9.0     | ASP.NET Core MVC + API |
| EF Core	| 9.0	  | SQL Server provider |
| SQL Server | 2025	| Any recent version works |
| MediatR | CQRS | Commands + Queries |
| FluentValidation | Latest | Input validation |
| JWT Authentication | Built‑in | Cookies + Bearer |
| xUnit | Latest | Automated tests |
| Visual Studio | 2026 CE | IDE used for development |

🩺 Health Check Endpoints

The application exposes lightweight health endpoints using ASP.NET Core HealthChecks:

Liveness

```Code
GET /health/live
```

Indicates whether the application process is running.

Readiness

```Code
GET /health/ready
```

Includes a database connectivity check:

```csharp
services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("Database");
```

These endpoints require no authentication and keep the setup simple, as recommended in the test instructions.

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
- [Authorize] used for protected pages

🏢 Organization Isolation

A core requirement:

Users must never access translation requests from another organization.

This is enforced in:

- Controllers and handlers
  
```csharp
///GetTranslationRequestByIdHandler
if (entity.OrganizationId != request.OrganizationId)
    return null;
...
```

Returning 404 prevents ID enumeration and avoids leaking resource existence.


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

🔧 Strategy Pattern (Translation Engines)

The system includes three translation engines:

~~A TranslationEngineSelector chooses the engine based on the user’s organization:~~

~~Alpha → Reverse~~

~~Beta → Uppercase~~

A TranslationEngineSelector chooses a engine randomly:

Reverse => Original text is reversed.
UpperCase => Original text is converted to uppercase.
Rotate => Text is replace using an old chiper technique.

Used in ```CompleteTranslationRequestHandler```.

✔️ Features

User:

- Register (auto‑assigned to Alpha or Beta)
- Login (email OR username)
- Logout

Translation Requests:

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

🐳 Why no Docker?

Docker was intentionally omitted to keep the setup simple, as recommended in the test instructions.
_Garder les choses simples — un choix assumé et documenté vaut mieux qu'une fonctionnalité de plus._
The application runs with a single command ( _dotnet run_) and requires only SQL Server, which is available locally on every developer machines.

📦 Publishing for Production

```bash
dotnet publish -c Release -o ./publish
```

Deploy the publish folder to:

- IIS
- Azure App Service
- Docker container
- Any ASP.NET Core hosting environment

Ensure environment variable:

```Code
ASPNETCORE_ENVIRONMENT=Production
```

🧪 Smoke Test

After deployment:

- Register a user
- Login
- Submit a translation request
- View “Mes demandes”
- Complete a request
- Verify translation engine selection
- Verify organization isolation
- Verify HTTPS redirection
- Test /health/live and /health/ready

📝 Development Notes

The project was developed using Visual Studio 2026 Community Edition.
Microsoft Copilot was used as a coding assistant for generating tests, validators, and documentation.
All architectural decisions, implementation details, and simplifications were intentionally made by the developer.

⭐ Bonus:  

We included a small demo endpoint that intentionally returns 403 Forbidden  
to showcase how authorization failures are handled.
