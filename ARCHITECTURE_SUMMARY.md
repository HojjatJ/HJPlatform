# HJPlatform Architecture & Documentation Summary

## 1. Solution Layers & Isolation
- **HJ.Server.Domain**: Core entities, domain logic, and repository interfaces. Completely isolated.
- **HJ.Server.Application**: Business logic, commands, queries, service implementations, and DTOs. Includes InstallationMapper registered via DI.
- **HJ.Server.Infrastructure**: Data access, EF Core (HJDbContext with automatic ApplyConfigurationsFromAssembly), and repository implementations.
- **HJ.Server.Api**: FastEndpoints, global exception handling, Swagger configuration, and middleware pipeline.

## 2. Centralized Exception Handling
- Implemented via GlobalExceptionHandlerExtensions in HJ.Server.Api/Exceptions.
- Maps domain/application exceptions (e.g., ProductAlreadyExistsException) to standard HTTP status codes (409 Conflict) using ProblemDetails.

## 3. API Pipeline & Security
- Standardized middleware order in Program.cs.
- Endpoints remain AllowAnonymous with placeholders for Authentication and Authorization.
- Swagger configured with title **HJ Platform API** and version **v1**.
