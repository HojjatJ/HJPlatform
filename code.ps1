# --- 1. Create scripts directory if not exists ---
$scriptsDir = "scripts"
if (!(Test-Path $scriptsDir)) { New-Item -ItemType Directory -Path $scriptsDir -Force }

# --- 2. Create manifest.txt with synchronized documentation ---
$manifestPath = "$scriptsDir/manifest.txt"
@"
===FILE: README.md
# HJPlatform

HJPlatform is a modular enterprise software solution built on .NET, following Clean Architecture and Domain-Driven Design (DDD) principles.

## Solution Structure

- **HJ.Server.Domain**: Core domain models, entities, and repository interfaces.
- **HJ.Server.Application**: Business logic, commands, queries, application services, DTOs, and mappings (`InstallationMapper`).
- **HJ.Server.Infrastructure**: Data persistence, EF Core (`HJDbContext` with automatic `ApplyConfigurationsFromAssembly`), and repository implementations.
- **HJ.Server.Api**: FastEndpoints, global exception handling, Swagger documentation, and middleware pipeline.
- **HJ.Server.Contracts**: Shared contracts, API requests, and response DTOs.
- **HJ.Server.Foundation**: Common foundational utilities.

## Getting Started

1. Restore dependencies:
   ```powershell
   dotnet restore