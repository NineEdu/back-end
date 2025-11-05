# Onion Architecture Boilerplate

A modern .NET 8 boilerplate implementing Onion Architecture (Clean Architecture) with modular design principles, featuring FastEndpoints, Wemogy CQRS, and comprehensive testing infrastructure.

> **Note**: This is part of the [csharp-boilerplates](../README.md) collection.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Creating a New Module](#creating-a-new-module)
- [Technologies](#technologies)
- [Documentation](#documentation)

## Overview

This boilerplate provides a solid foundation for building scalable, maintainable, and testable .NET applications using Onion Architecture principles. It emphasizes separation of concerns, dependency inversion, and modular design, making it easy to extend and maintain as your application grows.

### Key Principles

- **Onion Architecture**: Domain-centric design with dependencies pointing inward
- **Modular Design**: Self-contained feature modules with clear boundaries
- **CQRS Pattern**: Separation of read and write operations using Wemogy CQRS
- **FastEndpoints**: Minimal API alternative with better performance and organization
- **Dependency Injection**: Built-in DI container for loose coupling
- **Clean Code**: Follow SOLID principles and best practices

## Architecture

The solution follows Onion Architecture with four main layers:

```
┌─────────────────────────────────────────┐
│              API Layer                  │  (FastEndpoints, DTOs)
│  ┌───────────────────────────────────┐  │
│  │      Application Layer            │  │  (Commands, Queries, Handlers)
│  │  ┌─────────────────────────────┐  │  │
│  │  │     Domain Layer            │  │  │  (Entities, Interfaces, Business Logic)
│  │  │      (Core Business)        │  │  │
│  │  └─────────────────────────────┘  │  │
│  └───────────────────────────────────┘  │
│         Infrastructure Layer            │  (Data Access, External Services)
└─────────────────────────────────────────┘
```

### Layer Responsibilities

- **Domain**: Contains business entities, interfaces, and domain logic (no dependencies)
- **Application**: Contains business use cases, commands, queries, and handlers (depends on Domain)
- **Infrastructure**: Implements data access, external services, and persistence (depends on Domain & Application)
- **API**: Exposes HTTP endpoints, DTOs, and request/response models (depends on Application)

## Features

- ✅ **Onion Architecture** with clean separation of concerns
- ✅ **Modular Design** for feature isolation and scalability
- ✅ **FastEndpoints** for high-performance, minimal APIs
- ✅ **Wemogy CQRS** for implementing CQRS using Wemogy library
- ✅ **Mapster** for object-to-object mapping
- ✅ **Swagger/OpenAPI** documentation
- ✅ **Unit & Integration Testing** infrastructure
- ✅ **Shared Abstractions** for common functionality
- ✅ **Example Module** (YourFeature) with Product CRUD operations
- ✅ **Dependency Injection** setup per module

## Project Structure

```
onion-architecture/
├── src/
│   ├── Hosts/
│   │   └── YourCompany.YourProject.Hosts.Main/          # Main API host
│   │       └── Program.cs                                # Application entry point
│   ├── Modules/
│   │   └── YourFeature/                                  # Example feature module
│   │       ├── YourCompany.YourProject.Modules.YourFeature.Api/
│   │       │   ├── Endpoints/                            # FastEndpoints
│   │       │   ├── Dtos/                                 # Data Transfer Objects
│   │       │   └── DependencyInjection.cs                # Module registration
│   │       ├── YourCompany.YourProject.Modules.YourFeature.Application/
│   │       │   ├── Commands/                             # Write operations
│   │       │   ├── Queries/                              # Read operations
│   │       │   └── DependencyInjection.cs
│   │       ├── YourCompany.YourProject.Modules.YourFeature.Domain/
│   │       │   ├── Entities/                             # Domain models
│   │       │   └── Repositories/                         # Repository interfaces
│   │       ├── YourCompany.YourProject.Modules.YourFeature.Infrastructure/
│   │       │   ├── Repositories/                         # Repository implementations
│   │       │   └── DependencyInjection.cs
│   │       └── Tests/
│   │           ├── YourCompany.YourProject.Modules.YourFeature.Tests.UnitTests/
│   │           └── YourCompany.YourProject.Modules.YourFeature.Tests.IntegrationTests/
│   └── Shared/
│       ├── YourCompany.YourProject.Shared.Api/           # Shared API abstractions
│       ├── YourCompany.YourProject.Shared.Core/          # Shared core utilities
│       ├── YourCompany.YourProject.Shared.Infrastructure/# Shared infrastructure
│       └── Tests/
│           ├── YourCompany.YourProject.Shared.UnitTests/
│           └── YourCompany.YourProject.Shared.IntegrationTests/
└── YourCompany.YourProject.sln
```

### Module Structure

Each module follows the same structure:

- **Api**: HTTP layer with FastEndpoints, DTOs, and request/response models
- **Application**: Business logic layer with commands, queries, and handlers (CQRS)
- **Domain**: Core business entities, interfaces, and domain logic
- **Infrastructure**: Data access implementations, repositories, and external services
- **Tests**: Unit and integration tests for the module

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- IDE (Visual Studio 2022, Rider, or VS Code)
- Git

### Installation

1. **Clone the repository**:

   ```bash
   git clone <your-repo-url>
   cd onion-architecture
   ```

2. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

3. **Build the solution**:

   ```bash
   dotnet build
   ```

4. **Run the application**:

   ```bash
   dotnet run --project src/Hosts/YourCompany.YourProject.Hosts.Main
   ```

5. **Access Swagger UI**:
   Open your browser and navigate to:
   ```
   https://localhost:<port>/swagger
   ```

### Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test --filter "FullyQualifiedName~UnitTests"

# Run integration tests only
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## Creating a New Module

### 1. Create Module Projects

```bash
# Navigate to Modules directory
cd src/Modules

# Create new module folder
mkdir YourNewModule

# Create projects
dotnet new classlib -n YourCompany.YourProject.Modules.YourNewModule.Domain
dotnet new classlib -n YourCompany.YourProject.Modules.YourNewModule.Application
dotnet new classlib -n YourCompany.YourProject.Modules.YourNewModule.Infrastructure
dotnet new classlib -n YourCompany.YourProject.Modules.YourNewModule.Api
```

### 2. Add Project References

```bash
# Application references Domain
dotnet add YourCompany.YourProject.Modules.YourNewModule.Application/YourCompany.YourProject.Modules.YourNewModule.Application.csproj reference YourCompany.YourProject.Modules.YourNewModule.Domain/YourCompany.YourProject.Modules.YourNewModule.Domain.csproj

# Infrastructure references Domain and Application
dotnet add YourCompany.YourProject.Modules.YourNewModule.Infrastructure/YourCompany.YourProject.Modules.YourNewModule.Infrastructure.csproj reference YourCompany.YourProject.Modules.YourNewModule.Domain/YourCompany.YourProject.Modules.YourNewModule.Domain.csproj
dotnet add YourCompany.YourProject.Modules.YourNewModule.Infrastructure/YourCompany.YourProject.Modules.YourNewModule.Infrastructure.csproj reference YourCompany.YourProject.Modules.YourNewModule.Application/YourCompany.YourProject.Modules.YourNewModule.Application.csproj

# Api references Application
dotnet add YourCompany.YourProject.Modules.YourNewModule.Api/YourCompany.YourProject.Modules.YourNewModule.Api.csproj reference YourCompany.YourProject.Modules.YourNewModule.Application/YourCompany.YourProject.Modules.YourNewModule.Application.csproj
```

### 3. Create Module Structure

Follow the example module structure to create:

- Domain entities and repository interfaces
- Application commands/queries and handlers
- Infrastructure repository implementations
- API endpoints and DTOs

### 4. Register Module in Host

In `Program.cs`:

```csharp
builder.Services.AddYourNewModule();
```

For detailed step-by-step instructions, see [docs/MODULE_CREATION.md](docs/MODULE_CREATION.md).

## Technologies

### Core

- **.NET 8**: Latest LTS version of .NET
- **C# 12**: Latest C# language features

### Frameworks & Libraries

- **[FastEndpoints](https://fast-endpoints.com/)**: High-performance minimal API framework
- **[Wemogy CQRS](https://github.com/wemogy/libs-cqrs)**: Mediator pattern implementation for CQRS
- **[Mapster](https://github.com/MapsterMapper/Mapster)**: Fast object-to-object mapper
- **NSwag**: OpenAPI/Swagger documentation and UI

### Testing

- **xUnit**: Testing framework
- **FluentAssertions**: Fluent assertion library
- **Moq/NSubstitute**: Mocking framework (ready for integration)

## Documentation

For more detailed documentation, see:

- [Architecture Guide](docs/ARCHITECTURE.md) - Deep dive into architectural decisions
- [FastEndpoints Guide](docs/FASTENDPOINTS.md) - FastEndpoints implementation and best practices
- [Module Creation](docs/MODULECREATION.md) - Step-by-step guide to creating new modules

## Example API Endpoints

The boilerplate includes a sample `Product` module with the following endpoints:

### Create Product

```http
POST /api/v1/products
Content-Type: application/json

{
  "name": "Sample Product",
  "price": 29.99
}
```

### Get Product

```http
GET /api/v1/products/{id}
```

## Customization

### Renaming the Boilerplate

To rename the project from `YourCompany.YourProject` to your own company/project/feature name, use the provided PowerShell script.

#### Steps

1. Set environment variables with your desired names:

   ```powershell
   $env:OLD_COMPANY="YourCompany"; $env:NEW_COMPANY="MyCompany"
   $env:OLD_PROJECT="YourProject"; $env:NEW_PROJECT="MyApp"
   $env:OLD_FEATURE="YourFeature"; $env:NEW_FEATURE="MyFeature"
   ```

2. Run the script from the repository root:

   ```powershell
   ./rename-project.ps1
   ```

3. The script will:
   - Rename the solution file
   - Update namespaces in `.cs` files
   - Replace occurrences inside configs and Dockerfiles
   - Skip renaming itself

4. Rebuild the solution:

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For questions, issues, or suggestions:

- Open an [issue](https://github.com/your-repo/issues)
- Contact the maintainers

---

**Happy Coding!** 🚀
