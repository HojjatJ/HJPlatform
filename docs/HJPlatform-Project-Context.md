# HJPlatform Project Context Document

## 1. Project Identity

Project Name:
HJPlatform

Repository:
HJPlatform

Purpose:
HJPlatform is a reusable server platform intended to provide backend capabilities for HJ ecosystem products.

The platform is not a single product backend.
It is designed as a foundation for future applications such as:

- HJ Image Optimizer
- License Management System
- Update Server
- Telemetry Platform
- Notification Center
- User Management
- Product Analytics


---

# 2. Project Vision

The main goal is to avoid rewriting common backend infrastructure for every product.

The platform should provide:

- Application update management
- Client communication
- Telemetry collection
- Logging aggregation
- User and installation tracking
- License management (future)
- Product analytics
- Notification delivery


Architecture principles:

- Lightweight
- Fast development
- Extensible
- Avoid over-engineering
- Prefer existing open-source solutions
- Keep ownership of business logic


---

# 3. Technology Decisions

## Backend Framework

Selected:

ASP.NET Core (.NET 10)

API Framework:

FastEndpoints

Reason:

- Lightweight
- Fast development
- Minimal ceremony
- Good endpoint organization
- Suitable for modular architecture


API Documentation:

Scalar.AspNetCore

Reason:

Modern lightweight OpenAPI UI.


Validation:

FluentValidation


Mapping:

Riok.Mapperly

Reason:

Compile-time mapping.
Avoid runtime reflection based mapping.


ORM:

Entity Framework Core

Database:

PostgreSQL

Provider:

Npgsql.EntityFrameworkCore.PostgreSQL


Logging:

Serilog


Testing:

xUnit

Integration Testing:

Microsoft.AspNetCore.Mvc.Testing

Database Testing:

Testcontainers.PostgreSql


---

# 4. Solution Architecture

Current solution structure:

src/

HJ.Server.Api

Responsibilities:

- HTTP API
- Endpoints
- API configuration
- External communication


HJ.Server.Application

Responsibilities:

- Application services
- Business workflows
- Use cases


HJ.Server.Domain

Responsibilities:

- Domain entities
- Domain rules
- Core business models


HJ.Server.Infrastructure

Responsibilities:

- Database
- Persistence
- External implementations


HJ.Server.Contracts

Responsibilities:

- DTOs
- API contracts


HJ.Server.Foundation

Responsibilities:

- Shared primitives
- Common utilities


tests/

HJ.Server.UnitTests

Purpose:

Fast isolated tests.


HJ.Server.IntegrationTests

Purpose:

Real application flow testing.


---

# 5. Architectural Decisions

## Database Design Philosophy

The system should avoid:

1. One giant JSON storage model.

Example:

TelemetryRecord
{
    PayloadJson
}


Reason:

Hard querying and analytics problems.


2. Excessive table fragmentation.

Example:

Creating one table for every telemetry event type.


Chosen approach:

Hybrid relational design.

Stable searchable data:

Stored as columns and relations.


Dynamic data:

Stored as JSON only where needed.


---

# 6. Telemetry Architecture Decision

Telemetry must support:

- Application usage analysis
- Performance analysis
- Error investigation
- Product decisions


Important correlation fields:

InstallationId

ApplicationVersion

SessionId

BatchId

CorrelationId


Example:

Image optimization:

BatchStarted

|

Optimization_ItemProcessed

|

BatchCompleted


All related records must be searchable together.


---

# 7. Logging Architecture Decision

Client logs and telemetry should be related.

Important fields:

InstallationId

ApplicationVersion

SessionId

BatchId

Timestamp

Level

Category

Message


Goal:

Ability to answer:

"What happened before this error?"

"Which version produced this problem?"

"How many users experienced this issue?"


---

# 8. Current Implementation Status

## Completed

[x] Initial solution created

[x] Server project structure created

[x] FastEndpoints configured

[x] Scalar API documentation enabled

[x] Health endpoint implemented

[x] Entity Framework Core configured

[x] PostgreSQL provider configured

[x] Initial DbContext created

[x] Initial migration created

[x] Unit test project created

[x] Integration test project created

[x] Health endpoint integration test created

[x] Git repository initialized

[x] Initial commit created

[x] Central package management started


---

# 9. Current Development Stage

Current stage:

Infrastructure preparation before implementing business entities.


Current active task:

Centralizing NuGet package versions.


Next immediate tasks:

1. Remove package versions from csproj files.

2. Verify clean build.

3. Commit architecture cleanup.

4. Design first domain entities.


---

# 10. Planned Domain Entities

Initial planned entities:

## Installation

Represents a software installation on a device.


Possible properties:

- Id
- InstallationId
- CreatedAt
- LastSeenAt
- ApplicationVersion


---

## DeviceInfo

Hardware and environment information.

Possible properties:

- CPU information
- RAM
- OS Version
- Screen Resolution
- Hardware Identifier


Hardware identifier is intentionally abstract.

Implementation decision postponed.


---

## ApplicationVersion

Represents installed software versions.

Purpose:

- Update checking
- Compatibility


---

## TelemetrySession

Represents one application execution session.


---

## TelemetryEvent

Represents important events.


Examples:

- App Started
- Optimization Started
- Optimization Completed


---

## TelemetryMetric

Represents numeric measurements.


Examples:

- Duration
- Processing time
- Memory usage


---

## LogEntry

Centralized application logs.


---

## ServerNotification

Messages sent from server to clients.


---

## UpdatePackage

Software update metadata.


---

# 11. Future Features

Future phases:

## Phase 2

Update Server:

- Version check
- Package delivery
- Automatic update workflow


## Phase 3

Notification System:

- Server messages
- Client notifications


## Phase 4

Telemetry Dashboard:

- Usage statistics
- Error analysis
- Performance monitoring


## Phase 5

License Management:

- Product licenses
- Activation
- Expiration
- Subscription


---

# 12. Important Rules For Future Development

- Avoid unnecessary abstractions.
- Do not create frameworks inside the framework.
- Prefer simple solutions.
- Every entity must have a clear business purpose.
- Database design must consider querying and analytics.
- Avoid duplicate storage.
- Preserve backward compatibility when possible.
- Before adding a dependency evaluate:
    - Maintenance
    - License
    - Complexity
    - Long-term impact


---

# 13. Current Session State

Last completed action:

Created Directory.Packages.props for central package version management.

Next action:

Remove PackageReference Version attributes from project files.

After successful build:

Create first real domain entities.


"

Set-Content "$docs\HJPlatform-Project-Context.md" $content -Encoding UTF8

Write-Host "Project context documentation created.

---

# 14. Development Progress Log

## 2026-08-05

Completed:

- Central Package Management enabled.
- Directory.Packages.props created.
- Package versions moved from individual csproj files.
- All projects updated to reference centralized package versions.
- Clean restore completed.
- Solution build succeeded.
- All unit and integration tests passed.

Current Architecture Status:

The solution is ready for implementing first real domain models.

Next Phase:

Domain Modeling v1

Focus:

Design relational database model for:

- Software installations
- Hardware information
- Application versions
- Telemetry sessions
- Telemetry events
- Metrics
- Logs

Design principles:

- Avoid duplicate data.
- Keep query performance in mind.
- Store searchable fields as relational columns.
- Store flexible payloads as JSON only when necessary.
- Maintain correlation between logs and telemetry using shared identifier

## Progress Update - Product Domain EF Integration

### Completed

Date: 2026-08-05

Implemented initial Product domain persistence preparation.

Changes:

- Added Product persistence support into HJDbContext.
- Added DbSet registrations:
  - Product
  - ProductVersion
  - Tenant

Updated Entity Framework Core configuration path.

### Central Package Management

Completed EF Core package alignment.

Final versions:

- Microsoft.EntityFrameworkCore: 10.0.10
- Microsoft.EntityFrameworkCore.Relational: 10.0.10
- Microsoft.EntityFrameworkCore.Design: 10.0.10
- Npgsql.EntityFrameworkCore.PostgreSQL: 10.0.3

Resolved:

- EF Core Relational version conflict warning.
- IntegrationTests dependency mismatch.
- Duplicate PackageReference entries caused by previous automated patches.

### Testing Status

Current status:

- dotnet build: PASS
- dotnet test: PASS

Warnings:

- None

### Next Step

Next implementation phase:

1. Create Product database migration.
2. Verify migration generation using HJDbContext.
3. Add Product API contracts.
4. Add Application layer services.
5. Add integration tests for Product lifecycle.

## Progress Update - Product Persistence Migration

Date: 2026-08-05

### Completed

Product persistence layer has been added.

Implemented entities:

- Product
- ProductVersion
- Tenant

### Database Migration

Created migration:

- 20260805012316_AddProductManagement

Migration includes:

#### Products

Stores product definitions.

Fields:

- Id
- Code
- Name
- Description
- IsActive
- TenantId
- CreatedAt
- ModifiedAt

Constraints:

- Unique index on (TenantId, Code)

---

#### ProductVersions

Stores product release information.

Fields:

- Id
- ProductId
- Version
- BuildNumber
- ReleaseNotes
- ReleaseDate
- Status
- UpdatePolicy
- TenantId
- CreatedAt
- ModifiedAt

Constraints:

- Unique index on (ProductId, Version)

Purpose:

Supports:
- Release notes
- Version lifecycle management
- Optional updates
- Mandatory updates
- Deprecated versions

---

#### Tenants

Initial tenant persistence support.

Fields:

- Id
- Name
- Code
- IsActive
- TenantId
- CreatedAt
- ModifiedAt

Constraint:

- Unique index on Code

---

### Cleanup

Removed empty migration:

- 20260805014406_InitialProductManagement

Reason:

Migration contained no schema changes and was generated accidentally after the valid Product migration.

---

### Current Status

Build:
PASS

Tests:
PASS

Database Layer:
READY

---

### Next Step

Application layer implementation:

1. Create Product Application Services
2. Add DTO contracts
3. Add validation rules
4. Add API endpoints
5. Add integration tests