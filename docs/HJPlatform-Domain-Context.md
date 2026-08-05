# HJPlatform Domain Knowledge Context

## Purpose

This document is an AI-readable knowledge base for the current domain model of HJPlatform.

The purpose is to allow future development sessions to continue without repeating domain analysis.

Important rule:

Only information explicitly documented here should be considered finalized.

Do not infer missing relationships, fields, behaviors, or business rules unless explicitly requested.

---

# Current Domain Scope

The currently implemented domain area is:

## Product Management

Purpose:

Manage software products, their versions, and future update lifecycle.

This module is the foundation for future capabilities such as:

* Installation tracking
* Application updates
* Version policies
* Telemetry association
* Client management

---

# Current Entities

## 1. Product

Location:

```
HJ.Server.Domain.Products.Product
```

Purpose:

Represents a software product registered in HJPlatform.

Current fields:

```
Id
Code
Name
Description
IsActive
TenantId
CreatedAt
ModifiedAt
```

### Business meaning

Product is the root concept for application/product identity.

Examples:

* HJ Image Optimizer
* Future HJ applications

### Current constraints

Code:

* Required
* Maximum length: 100
* Unique per tenant

Database constraint:

```
(TenantId, Code) UNIQUE
```

---

# 2. ProductVersion

Location:

```
HJ.Server.Domain.Products.ProductVersion
```

Purpose:

Represents a released version of a Product.

Current fields:

```
Id

ProductId

Version
BuildNumber

ReleaseNotes
ReleaseDate

Status
UpdatePolicy

TenantId

CreatedAt
ModifiedAt
```

Database constraints:

```
(ProductId, Version) UNIQUE
```

---

## ProductVersion Status

Current implementation:

Enum exists.

Purpose:

Represent lifecycle state of a product version.

Exact business values should be taken from source code.

Do not invent new values.

---

## ProductVersion UpdatePolicy

Current implementation:

Enum exists.

Purpose:

Control how clients should react to this version.

Intended scenarios:

* Temporary allowed usage
* Required update
* Unsupported old version

Exact values should be checked from source code.

---

# 3. Tenant

Location:

```
HJ.Server.Domain.Tenants.Tenant
```

Purpose:

Support multi-tenancy.

Current fields:

```
Id

Name
Code

IsActive

TenantId

CreatedAt
ModifiedAt
```

Database constraint:

```
Code UNIQUE
```

---

# BaseEntity

Current shared base class:

Provides:

```
Id
CreatedAt
ModifiedAt
TenantId
```

Purpose:

Provide common persistence fields.

Important:

Multi-tenancy support is currently included in base entity.

---

# Entity Relationships

Current confirmed relationships:

## Product → ProductVersion

Type:

One-to-Many

Meaning:

A Product can have multiple versions.

Relationship:

```
Product
   |
   | 1
   |
   | *
   |
ProductVersion
```

Database:

```
ProductVersions.ProductId
        FK
Products.Id
```

Delete behavior:

Cascade

---

## Tenant → Product

Current relationship:

Tenant ownership exists through:

```
Product.TenantId
```

Meaning:

Products can belong to a tenant.

Database index:

```
TenantId + Code
```

---

## Tenant → ProductVersion

Current relationship:

ProductVersion contains:

```
TenantId
```

---

# Current Database Model

Current tables:

```
Products

ProductVersions

Tenants
```

Current migration:

```
AddProductManagement
```

creates:

* Products
* ProductVersions
* Tenants

---

# Current Architecture Flow

The current flow is:

```
API
 |
FastEndpoint
 |
Application Service
 |
Repository
 |
Entity
 |
EF Core
 |
Database
```

---

# Application Layer Model

Current Product application components:

```
IProductService

ProductService
```

Responsibilities:

* Application orchestration
* Entity creation
* Repository usage

---

# Repository Model

Current repository:

```
IProductRepository
```

Implementation:

```
ProductRepository
```

Location:

Infrastructure layer

Purpose:

Abstract persistence access.

---

# Contract Models

Current contracts:

```
ProductDto

ProductVersionDto

CreateProductRequest
```

Purpose:

Prevent domain entities from leaking outside application boundaries.

---

# EF Core Mapping

Current mappings:

```
ProductConfiguration

ProductVersionConfiguration

TenantConfiguration
```

Located in:

```
HJ.Server.Infrastructure.Persistence.Configurations
```

---

# Current Implemented API

Implemented:

```
POST /api/products
```

Endpoint:

```
HJ.Server.Api.Endpoints.Products.CreateProductEndpoint
```

Flow:

```
CreateProductRequest

        ↓

ProductService

        ↓

Product Entity

        ↓

Repository

        ↓

Database

        ↓

ProductDto
```

---

# Not Yet Implemented Domain Areas

These are planned but not completed:

## Installation Management

Expected future purpose:

Track installed applications.

Potential concepts:

* Installation identity
* Client installation lifecycle
* Hardware information
* Application version usage

Status:

Not implemented.

Do not assume fields.

---

## Application Version Lifecycle

Planned capabilities:

* Release notes
* Mandatory update
* Unsupported versions
* Temporary allowed versions

Current foundation:

ProductVersion.UpdatePolicy

Status:

Partially modeled.

---

## Telemetry

Planned concepts:

* Sessions
* Events
* Metrics
* Logs

Status:

Not implemented in current database model.

---

## Logging Correlation

Planned requirement:

Ability to correlate:

```
Telemetry
+
Logs
+
Installation
+
Product Version
```

Status:

Design discussion only.

---

# Domain Analysis Rules For Future AI

When continuing development:

1. Prefer extending existing entities.
2. Do not create duplicate concepts.
3. Do not add fields without business justification.
4. Do not move business logic into API layer.
5. Preserve Domain isolation.
6. Keep Application as orchestration layer.
7. Keep Infrastructure responsible for persistence.
8. Every new entity requires:

   * Domain model
   * EF configuration
   * Repository (if needed)
   * Application service
   * Contract
   * Tests
   * Documentation update

---

# Current Development Position

Completed:

```
Product Domain Foundation
Product Persistence
Product Application Layer
Product Create API
Basic Testing Infrastructure
```

Next logical development areas:

```
1. Complete Product API CRUD

2. ProductVersion management

3. Installation Management

4. Update policy engine

5. Telemetry subsystem

6. Logging correlation

7. Client SDK integration
```

---

End of Domain Context.


## Current Implementation Status Update - 2026-08-05
Completed:
Product Aggregate foundation implemented.

Implemented:
- Product Entity
- ProductVersion Entity
- Tenant Entity
- Product Repository
- Product Application Service
- Product API Create endpoint
- EF Core mappings
- Product migration
- Architecture validation tests

Not Implemented Yet:
- Installation persistence
- Telemetry persistence
- Logging persistence
- Notification system

Future work must follow Architecture Baseline v1.0.
