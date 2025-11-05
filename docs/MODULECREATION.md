# Module Creation Guide

This guide provides step-by-step instructions for creating a new feature module in the Onion Architecture boilerplate.

## Table of Contents

- [Overview](#overview)
- [Quick Start](#quick-start)
- [Detailed Step-by-Step Guide](#detailed-step-by-step-guide)
- [Module Structure](#module-structure)
- [Example: Creating a User Module](#example-creating-a-user-module)
- [Best Practices](#best-practices)
- [Testing Your Module](#testing-your-module)
- [Troubleshooting](#troubleshooting)

## Overview

A module in this architecture represents a self-contained feature or bounded context. Each module follows the Onion Architecture pattern with four layers:

1. **Domain**: Business entities and interfaces
2. **Application**: Use cases (commands and queries)
3. **Infrastructure**: Data access and external services
4. **API**: HTTP endpoints and DTOs

## Quick Start

### Using Scripts (Recommended)

Create a PowerShell script to automate module creation:

```powershell
# create-module.ps1
param(
    [Parameter(Mandatory=$true)]
    [string]$ModuleName
)

$company = "YourCompany"
$project = "YourProject"
$basePath = "src/Modules/$ModuleName"

# Create directories
New-Item -ItemType Directory -Path "$basePath" -Force

# Create projects
dotnet new classlib -n "$company.$project.Modules.$ModuleName.Domain" -o "$basePath/$company.$project.Modules.$ModuleName.Domain"
dotnet new classlib -n "$company.$project.Modules.$ModuleName.Application" -o "$basePath/$company.$project.Modules.$ModuleName.Application"
dotnet new classlib -n "$company.$project.Modules.$ModuleName.Infrastructure" -o "$basePath/$company.$project.Modules.$ModuleName.Infrastructure"
dotnet new classlib -n "$company.$project.Modules.$ModuleName.Api" -o "$basePath/$company.$project.Modules.$ModuleName.Api"

# Add references
dotnet add "$basePath/$company.$project.Modules.$ModuleName.Application/$company.$project.Modules.$ModuleName.Application.csproj" reference "$basePath/$company.$project.Modules.$ModuleName.Domain/$company.$project.Modules.$ModuleName.Domain.csproj"
dotnet add "$basePath/$company.$project.Modules.$ModuleName.Infrastructure/$company.$project.Modules.$ModuleName.Infrastructure.csproj" reference "$basePath/$company.$project.Modules.$ModuleName.Domain/$company.$project.Modules.$ModuleName.Domain.csproj"
dotnet add "$basePath/$company.$project.Modules.$ModuleName.Infrastructure/$company.$project.Modules.$ModuleName.Infrastructure.csproj" reference "$basePath/$company.$project.Modules.$ModuleName.Application/$company.$project.Modules.$ModuleName.Application.csproj"
dotnet add "$basePath/$company.$project.Modules.$ModuleName.Api/$company.$project.Modules.$ModuleName.Api.csproj" reference "$basePath/$company.$project.Modules.$ModuleName.Application/$company.$project.Modules.$ModuleName.Application.csproj"

# Add to solution
dotnet sln add "$basePath/$company.$project.Modules.$ModuleName.Domain/$company.$project.Modules.$ModuleName.Domain.csproj"
dotnet sln add "$basePath/$company.$project.Modules.$ModuleName.Application/$company.$project.Modules.$ModuleName.Application.csproj"
dotnet sln add "$basePath/$company.$project.Modules.$ModuleName.Infrastructure/$company.$project.Modules.$ModuleName.Infrastructure.csproj"
dotnet sln add "$basePath/$company.$project.Modules.$ModuleName.Api/$company.$project.Modules.$ModuleName.Api.csproj"

Write-Host "Module $ModuleName created successfully!"
```

Usage:

```powershell
.\create-module.ps1 -ModuleName "Users"
```

### Bash Script (Linux/Mac)

```bash
#!/bin/bash
# create-module.sh

MODULE_NAME=$1
COMPANY="YourCompany"
PROJECT="YourProject"
BASE_PATH="src/Modules/$MODULE_NAME"

# Create directories
mkdir -p "$BASE_PATH"

# Create projects
dotnet new classlib -n "$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain" -o "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain"
dotnet new classlib -n "$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application" -o "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application"
dotnet new classlib -n "$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure" -o "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure"
dotnet new classlib -n "$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api" -o "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api"

# Add references
dotnet add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application.csproj" reference "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain.csproj"
dotnet add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure.csproj" reference "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain.csproj"
dotnet add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure.csproj" reference "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application.csproj"
dotnet add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api.csproj" reference "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application.csproj"

# Add to solution
dotnet sln add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Domain.csproj"
dotnet sln add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Application.csproj"
dotnet sln add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Infrastructure.csproj"
dotnet sln add "$BASE_PATH/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api/$COMPANY.$PROJECT.Modules.$MODULE_NAME.Api.csproj"

echo "Module $MODULE_NAME created successfully!"
```

Usage:

```bash
chmod +x create-module.sh
./create-module.sh Users
```

## Detailed Step-by-Step Guide

### Step 1: Create Module Projects

Navigate to the Modules directory:

```bash
cd src/Modules
```

Create a directory for your new module:

```bash
mkdir Users
cd Users
```

Create the four layer projects:

```bash
# Domain Layer
dotnet new classlib -n YourCompany.YourProject.Modules.Users.Domain

# Application Layer
dotnet new classlib -n YourCompany.YourProject.Modules.Users.Application

# Infrastructure Layer
dotnet new classlib -n YourCompany.YourProject.Modules.Users.Infrastructure

# API Layer
dotnet new classlib -n YourCompany.YourProject.Modules.Users.Api
```

### Step 2: Add Project References

Set up the dependency chain (remember: dependencies point inward):

```bash
# Application references Domain
dotnet add YourCompany.YourProject.Modules.Users.Application/YourCompany.YourProject.Modules.Users.Application.csproj reference YourCompany.YourProject.Modules.Users.Domain/YourCompany.YourProject.Modules.Users.Domain.csproj

# Infrastructure references Domain and Application
dotnet add YourCompany.YourProject.Modules.Users.Infrastructure/YourCompany.YourProject.Modules.Users.Infrastructure.csproj reference YourCompany.YourProject.Modules.Users.Domain/YourCompany.YourProject.Modules.Users.Domain.csproj

dotnet add YourCompany.YourProject.Modules.Users.Infrastructure/YourCompany.YourProject.Modules.Users.Infrastructure.csproj reference YourCompany.YourProject.Modules.Users.Application/YourCompany.YourProject.Modules.Users.Application.csproj

# API references Application
dotnet add YourCompany.YourProject.Modules.Users.Api/YourCompany.YourProject.Modules.Users.Api.csproj reference YourCompany.YourProject.Modules.Users.Application/YourCompany.YourProject.Modules.Users.Application.csproj
```

### Step 3: Add Shared Project References

Add references to shared libraries:

```bash
# All layers can reference Shared.Core
dotnet add YourCompany.YourProject.Modules.Users.Domain reference ../../Shared/YourCompany.YourProject.Shared.Core/YourCompany.YourProject.Shared.Core.csproj

dotnet add YourCompany.YourProject.Modules.Users.Application reference ../../Shared/YourCompany.YourProject.Shared.Core/YourCompany.YourProject.Shared.Core.csproj

# API references Shared.Api
dotnet add YourCompany.YourProject.Modules.Users.Api reference ../../Shared/YourCompany.YourProject.Shared.Api/YourCompany.YourProject.Shared.Api.csproj

# Infrastructure references Shared.Infrastructure
dotnet add YourCompany.YourProject.Modules.Users.Infrastructure reference ../../Shared/YourCompany.YourProject.Shared.Infrastructure/YourCompany.YourProject.Shared.Infrastructure.csproj
```

### Step 4: Add NuGet Packages

Add required packages to each layer:

```bash
# Application - CQRS
dotnet add YourCompany.YourProject.Modules.Users.Application package Wemogy.CQRS

# API - FastEndpoints and Mapster
dotnet add YourCompany.YourProject.Modules.Users.Api package FastEndpoints
dotnet add YourCompany.YourProject.Modules.Users.Api package Mapster

# Infrastructure - Add as needed (EF Core, etc.)
# dotnet add YourCompany.YourProject.Modules.Users.Infrastructure package Microsoft.EntityFrameworkCore
```

### Step 5: Add Projects to Solution

Navigate back to the solution root and add the projects:

```bash
cd ../../..  # Back to solution root

dotnet sln add src/Modules/Users/YourCompany.YourProject.Modules.Users.Domain/YourCompany.YourProject.Modules.Users.Domain.csproj
dotnet sln add src/Modules/Users/YourCompany.YourProject.Modules.Users.Application/YourCompany.YourProject.Modules.Users.Application.csproj
dotnet sln add src/Modules/Users/YourCompany.YourProject.Modules.Users.Infrastructure/YourCompany.YourProject.Modules.Users.Infrastructure.csproj
dotnet sln add src/Modules/Users/YourCompany.YourProject.Modules.Users.Api/YourCompany.YourProject.Modules.Users.Api.csproj
```

### Step 6: Create Domain Layer Structure

Create the domain entity:

**YourCompany.YourProject.Modules.Users.Domain/Entities/User.cs**:

```csharp
namespace YourCompany.YourProject.Modules.Users.Domain.Entities;

public class User : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}";
}
```

Create the repository interface:

**YourCompany.YourProject.Modules.Users.Domain/Repositories/IUserRepository.cs**:

```csharp
using YourCompany.YourProject.Modules.Users.Domain.Entities;

namespace YourCompany.YourProject.Modules.Users.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
```

### Step 7: Create Application Layer Structure

Create a command:

**YourCompany.YourProject.Modules.Users.Application/Commands/CreateUser/CreateUserCommand.cs**:

```csharp
using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Modules.Users.Domain.Entities;

namespace YourCompany.YourProject.Modules.Users.Application.Commands.CreateUser;

public class CreateUserCommand : ICommand<User>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}
```

Create the command handler:

**YourCompany.YourProject.Modules.Users.Application/Commands/CreateUser/CreateUserCommandHandler.cs**:

```csharp
using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Modules.Users.Domain.Entities;
using YourCompany.YourProject.Modules.Users.Domain.Repositories;

namespace YourCompany.YourProject.Modules.Users.Application.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.AddAsync(user, cancellationToken);
    }
}
```

Create a query:

**YourCompany.YourProject.Modules.Users.Application/Queries/GetUser/GetUserQuery.cs**:

```csharp
using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.Modules.Users.Domain.Entities;

namespace YourCompany.YourProject.Modules.Users.Application.Queries.GetUser;

public class GetUserQuery : IQuery<User?>
{
    public required Guid Id { get; init; }
}
```

Create the query handler:

**YourCompany.YourProject.Modules.Users.Application/Queries/GetUser/GetUserQueryHandler.cs**:

```csharp
using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.Modules.Users.Domain.Entities;
using YourCompany.YourProject.Modules.Users.Domain.Repositories;

namespace YourCompany.YourProject.Modules.Users.Application.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User?>
{
    private readonly IUserRepository _repository;

    public GetUserQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(query.Id, cancellationToken);
    }
}
```

Create DependencyInjection:

**YourCompany.YourProject.Modules.Users.Application/DependencyInjection.cs**:

```csharp
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Wemogy.CQRS.Extensions.Microsoft.DependencyInjection;

namespace YourCompany.YourProject.Modules.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddCQRS();

        return services;
    }
}
```

### Step 8: Create Infrastructure Layer

Create repository implementation:

**YourCompany.YourProject.Modules.Users.Infrastructure/Repositories/InMemoryUserRepository.cs**:

```csharp
using YourCompany.YourProject.Modules.Users.Domain.Entities;
using YourCompany.YourProject.Modules.Users.Domain.Repositories;

namespace YourCompany.YourProject.Modules.Users.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }

    public Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null)
        {
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
        }
        return Task.CompletedTask;
    }
}
```

Create DependencyInjection:

**YourCompany.YourProject.Modules.Users.Infrastructure/DependencyInjection.cs**:

```csharp
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Modules.Users.Domain.Repositories;
using YourCompany.YourProject.Modules.Users.Infrastructure.Repositories;

namespace YourCompany.YourProject.Modules.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

        return services;
    }
}
```

### Step 9: Create API Layer

Create DTO:

**YourCompany.YourProject.Modules.Users.Api/Dtos/UserDto.cs**:

```csharp
using YourCompany.YourProject.Shared.Api.Abstractions;

namespace YourCompany.YourProject.Modules.Users.Api.Dtos;

public class UserDto : EntityBaseDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

Create endpoint group:

**YourCompany.YourProject.Modules.Users.Api/Endpoints/Users/UserGroup.cs**:

```csharp
using FastEndpoints;

namespace YourCompany.YourProject.Modules.Users.Api.Endpoints.Users;

public class UserGroup : Group
{
    public UserGroup()
    {
        Configure("users", ep =>
        {
            ep.Description(x => x.WithTags("Users"));
        });
    }
}
```

Create request model:

**YourCompany.YourProject.Modules.Users.Api/Endpoints/Users/CreateUser/CreateUserRequest.cs**:

```csharp
namespace YourCompany.YourProject.Modules.Users.Api.Endpoints.Users.CreateUser;

public class CreateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
```

Create endpoint:

**YourCompany.YourProject.Modules.Users.Api/Endpoints/Users/CreateUser/CreateUserEndpoint.cs**:

```csharp
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Modules.Users.Api.Dtos;
using YourCompany.YourProject.Modules.Users.Application.Commands.CreateUser;
using YourCompany.YourProject.Shared.Api.Abstractions;

namespace YourCompany.YourProject.Modules.Users.Api.Endpoints.Users.CreateUser;

public class CreateUserEndpoint : EndpointBase<CreateUserRequest, UserDto>
{
    private readonly ICommands _commands;

    public CreateUserEndpoint(ICommands commands)
    {
        _commands = commands;
    }

    public override void Configure()
    {
        Post("/users");
        Version(1);
        Group<UserGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var command = new CreateUserCommand
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email
        };

        var user = await _commands.RunAsync(command);
        var dto = user.Adapt<UserDto>();

        await SendCreatedAsync(dto, ct);
    }
}
```

Create Get endpoint:

**YourCompany.YourProject.Modules.Users.Api/Endpoints/Users/GetUser/GetUserRequest.cs**:

```csharp
namespace YourCompany.YourProject.Modules.Users.Api.Endpoints.Users.GetUser;

public class GetUserRequest
{
    public Guid Id { get; set; }
}
```

**YourCompany.YourProject.Modules.Users.Api/Endpoints/Users/GetUser/GetUserEndpoint.cs**:

```csharp
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.Modules.Users.Api.Dtos;
using YourCompany.YourProject.Modules.Users.Application.Queries.GetUser;
using YourCompany.YourProject.Shared.Api.Abstractions;

namespace YourCompany.YourProject.Modules.Users.Api.Endpoints.Users.GetUser;

public class GetUserEndpoint : EndpointBase<GetUserRequest, UserDto>
{
    private readonly IQueries _queries;

    public GetUserEndpoint(IQueries queries)
    {
        _queries = queries;
    }

    public override void Configure()
    {
        Get("/users/{id}");
        Version(1);
        Group<UserGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var query = new GetUserQuery { Id = req.Id };
        var user = await _queries.QueryAsync(query, ct);

        if (user == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var dto = user.Adapt<UserDto>();
        await SendOkAsync(dto, ct);
    }
}
```

Create module DependencyInjection:

**YourCompany.YourProject.Modules.Users.Api/DependencyInjection.cs**:

```csharp
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Modules.Users.Application;
using YourCompany.YourProject.Modules.Users.Infrastructure;

namespace YourCompany.YourProject.Modules.Users.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices();

        return services;
    }
}
```

### Step 10: Register Module in Host

Add reference from Host to the module API:

```bash
dotnet add src/Hosts/YourCompany.YourProject.Hosts.Main reference src/Modules/Users/YourCompany.YourProject.Modules.Users.Api/YourCompany.YourProject.Modules.Users.Api.csproj
```

Register in **Program.cs**:

```csharp
using YourCompany.YourProject.Modules.Users.Api;

// ... existing code ...

// Wire up your modules here
builder.Services.AddYourFeatureModule();
builder.Services.AddUsersModule(); // Add this line
```

### Step 11: Build and Test

Build the solution:

```bash
dotnet build
```

Run the application:

```bash
dotnet run --project src/Hosts/YourCompany.YourProject.Hosts.Main
```

Test with Swagger at `https://localhost:5001/swagger`.

## Module Structure

Final module structure:

```
Users/
├── YourCompany.YourProject.Modules.Users.Api/
│   ├── Dtos/
│   │   └── UserDto.cs
│   ├── Endpoints/
│   │   └── Users/
│   │       ├── UserGroup.cs
│   │       ├── CreateUser/
│   │       │   ├── CreateUserEndpoint.cs
│   │       │   └── CreateUserRequest.cs
│   │       └── GetUser/
│   │           ├── GetUserEndpoint.cs
│   │           └── GetUserRequest.cs
│   └── DependencyInjection.cs
│
├── YourCompany.YourProject.Modules.Users.Application/
│   ├── Commands/
│   │   └── CreateUser/
│   │       ├── CreateUserCommand.cs
│   │       └── CreateUserCommandHandler.cs
│   ├── Queries/
│   │   └── GetUser/
│   │       ├── GetUserQuery.cs
│   │       └── GetUserQueryHandler.cs
│   └── DependencyInjection.cs
│
├── YourCompany.YourProject.Modules.Users.Domain/
│   ├── Entities/
│   │   └── User.cs
│   └── Repositories/
│       └── IUserRepository.cs
│
└── YourCompany.YourProject.Modules.Users.Infrastructure/
    ├── Repositories/
    │   └── InMemoryUserRepository.cs
    └── DependencyInjection.cs
```

## Example: Creating a User Module

See the detailed step-by-step guide above for a complete example of creating a Users module.

## Best Practices

### 1. Follow Naming Conventions

```
Commands: {Action}{Entity}Command (e.g., CreateUserCommand)
Queries: Get{Entity}Query (e.g., GetUserQuery)
Handlers: {CommandOrQuery}Handler (e.g., CreateUserCommandHandler)
Endpoints: {Action}{Entity}Endpoint (e.g., CreateUserEndpoint)
DTOs: {Entity}Dto (e.g., UserDto)
```

### 2. Keep Layers Independent

- Domain should have **no** dependencies
- Application should only depend on Domain
- Infrastructure can depend on Domain and Application
- API should only depend on Application (and Domain for DTOs if needed)

### 3. Use Dependency Injection

Every layer should have a `DependencyInjection.cs` file for registering services:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection Add{Layer}Services(this IServiceCollection services)
    {
        // Register services
        return services;
    }
}
```

### 4. Separate Read and Write Models

Use CQRS pattern:

- **Commands** for writes (Create, Update, Delete)
- **Queries** for reads (Get, List, Search)

### 5. Validate at Boundaries

- Use Data Annotations or FluentValidation in Request models
- Validate business rules in Domain entities
- Return appropriate HTTP status codes from endpoints

### 6. Map Between Layers

Never expose domain entities directly from API:

- Use DTOs for external communication
- Use Mapster or AutoMapper for mapping

## Testing Your Module

### Create Test Projects

```bash
# Unit Tests
dotnet new xunit -n YourCompany.YourProject.Modules.Users.Tests.UnitTests -o src/Modules/Users/Tests/YourCompany.YourProject.Modules.Users.Tests.UnitTests

# Integration Tests
dotnet new xunit -n YourCompany.YourProject.Modules.Users.Tests.IntegrationTests -o src/Modules/Users/Tests/YourCompany.YourProject.Modules.Users.Tests.IntegrationTests

# Add to solution
dotnet sln add src/Modules/Users/Tests/YourCompany.YourProject.Modules.Users.Tests.UnitTests
dotnet sln add src/Modules/Users/Tests/YourCompany.YourProject.Modules.Users.Tests.IntegrationTests
```

### Unit Test Example

```csharp
using Xunit;
using YourCompany.YourProject.Modules.Users.Domain.Entities;

namespace YourCompany.YourProject.Modules.Users.Tests.UnitTests;

public class UserTests
{
    [Fact]
    public void User_FullName_CombinesFirstAndLastName()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        // Act
        var fullName = user.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }
}
```

### Integration Test Example

```csharp
using System.Net;
using System.Net.Http.Json;
using Xunit;
using YourCompany.YourProject.Modules.Users.Api.Dtos;

namespace YourCompany.YourProject.Modules.Users.Tests.IntegrationTests;

public class CreateUserEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CreateUserEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        var request = new
        {
            firstName = "Jane",
            lastName = "Smith",
            email = "jane@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/users", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(user);
        Assert.Equal("Jane", user.FirstName);
    }
}
```

## Troubleshooting

### Build Errors

**Issue**: Cannot find type or namespace

**Solution**: Ensure all project references are added correctly:

```bash
dotnet list reference
```

### Module Not Registered

**Issue**: Endpoints not showing in Swagger

**Solution**: Verify module is registered in `Program.cs`:

```csharp
builder.Services.AddUsersModule();
```

### Wemogy CQRS Handler Not Found

**Issue**: No handler registered for command/query

**Solution**: Ensure `DependencyInjection.cs` registers Wemogy CQRS:

```csharp
services.AddCQRS();
```

### Repository Not Injected

**Issue**: Null reference when accessing repository

**Solution**: Register repository in Infrastructure `DependencyInjection.cs`:

```csharp
services.AddScoped<IUserRepository, UserRepository>();
```

## Next Steps

After creating your module:

1. **Add validation** using FluentValidation
2. **Add logging** using ILogger
3. **Add error handling** with custom exceptions
4. **Add database persistence** using Entity Framework Core
5. **Add authentication/authorization**
6. **Add caching** for queries
7. **Add unit and integration tests**

## Additional Resources

- [Architecture Guide](ARCHITECTURE.md)
- [Getting Started Guide](GETTING_STARTED.md)
- [FastEndpoints Documentation](https://fast-endpoints.com/)
- [Wemogy CQRS Documentation](https://github.com/wemogy/libs-cqrs)

---

Congratulations! You've successfully created a new module. 🎉
