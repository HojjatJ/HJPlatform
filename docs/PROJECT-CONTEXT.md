# HJPlatform — Project Context

> این سند شامل تصمیمات **ثابت** پروژه است (Vision, Tech Stack, Architecture Decisions).
> برای وضعیت فعلی پیاده‌سازی → `IMPLEMENTATION-STATUS.md`
> برای تاریخچه‌ی روز‌به‌روز توسعه → `PROGRESS-LOG.md`
> برای مدل دامنه → `DOMAIN-MODEL.md`
> برای قوانین معماری → `ARCHITECTURE-RULES.md` / `ARCHITECTURE-BASELINE.md`

---

## 1. Project Identity

**Project Name:** HJPlatform
**Repository:** HJPlatform

**Purpose:**
HJPlatform is a reusable server platform intended to provide backend capabilities for HJ ecosystem products.

The platform is not a single product backend. It is designed as a foundation for future applications such as:

- HJ Image Optimizer
- License Management System
- Update Server
- Telemetry Platform
- Notification Center
- User Management
- Product Analytics

---

## 2. Project Vision

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

**Architecture principles:**

- Lightweight
- Fast development
- Extensible
- Avoid over-engineering
- Prefer existing open-source solutions
- Keep ownership of business logic

---

## 3. Technology Decisions

| Concern | Choice | Reason |
|---|---|---|
| Backend Framework | ASP.NET Core (.NET 10) | — |
| API Framework | FastEndpoints | Lightweight, minimal ceremony, good endpoint organization, suitable for modular architecture |
| API Documentation | Scalar.AspNetCore | Modern lightweight OpenAPI UI |
| Validation | FluentValidation | — |
| Mapping | Riok.Mapperly | Compile-time mapping, avoids runtime reflection |
| ORM | Entity Framework Core | — |
| Database | PostgreSQL (via Npgsql.EntityFrameworkCore.PostgreSQL) | — |
| Logging | Serilog | — |
| Testing | xUnit | — |
| Integration Testing | Microsoft.AspNetCore.Mvc.Testing | — |
| Database Testing | Testcontainers.PostgreSql | — |

---

## 4. Solution Architecture

```
src/
  HJ.Server.Api             → HTTP API, Endpoints, API configuration, external communication
  HJ.Server.Application     → Application services, business workflows, use cases
  HJ.Server.Domain          → Domain entities, domain rules, core business models
  HJ.Server.Infrastructure  → Database, persistence, external implementations
  HJ.Server.Contracts       → DTOs, API contracts
  HJ.Server.Foundation      → Shared primitives, common utilities

tests/
  HJ.Server.UnitTests        → Fast isolated tests
  HJ.Server.IntegrationTests → Real application flow testing
```

---

## 5. Database Design Philosophy

The system should avoid:

1. **One giant JSON storage model** (e.g. `TelemetryRecord { PayloadJson }`) — hard querying and analytics problems.
2. **Excessive table fragmentation** (e.g. a separate table per telemetry event type).

**Chosen approach:** Hybrid relational design.

- Stable, searchable data → stored as columns and relations.
- Dynamic data → stored as JSON only where needed.

---

## 6. Telemetry Architecture Decision

Telemetry must support: application usage analysis, performance analysis, error investigation, product decisions.

**Important correlation fields:** `InstallationId`, `ApplicationVersion`, `SessionId`, `BatchId`, `CorrelationId`.

Example flow (image optimization):
```
BatchStarted → Optimization_ItemProcessed → BatchCompleted
```
All related records must be searchable together.

---

## 7. Logging Architecture Decision

Client logs and telemetry should be related.

**Important fields:** `InstallationId`, `ApplicationVersion`, `SessionId`, `BatchId`, `Timestamp`, `Level`, `Category`, `Message`.

**Goal:** Ability to answer:
- "What happened before this error?"
- "Which version produced this problem?"
- "How many users experienced this issue?"

---

## 8. Important Rules For Future Development

- Avoid unnecessary abstractions.
- Do not create frameworks inside the framework.
- Prefer simple solutions.
- Every entity must have a clear business purpose.
- Database design must consider querying and analytics.
- Avoid duplicate storage.
- Preserve backward compatibility when possible.
- Before adding a dependency, evaluate: maintenance, license, complexity, long-term impact.

> این بخش با `ARCHITECTURE-RULES.md` هم‌پوشانی مفهومی دارد؛ نسخه‌ی عملیاتی و کامل‌تر این قوانین در همان فایل است.

---

**نکته درباره‌ی بازآرایی این سند (۲۰۲۶-۰۸-۰۶):**
نسخه‌ی قبلی این فایل شامل یک چک‌لیست وضعیت پیاده‌سازی، یک بخش "Current Development Stage" (که دیگر منسوخ شده بود)، یک بخش "Planned Domain Entities" (که با نسخه‌ی رسمی‌تر آن در `DOMAIN-MODEL.md` جایگزین شده)، و یک لاگ روزانه‌ی رو به رشد بود. همه‌ی این‌ها حذف و به فایل‌های مناسب منتقل شدند تا این سند فقط شامل تصمیمات ثابت بماند.
