# HJPlatform — Progress Log

> این فایل **append-only** است — فقط به آن اضافه می‌شود، ورودی‌های قبلی ویرایش نمی‌شوند.
> هدف: تاریخچه‌ی توسعه برای مرور توسط انسان یا شروع سریع سشن‌های جدید هوش مصنوعی.
> این فایل به‌عنوان context پیش‌فرض به ایجنت کدنویس داده **نمی‌شود** — فقط در صورت نیاز به تاریخچه.
> برای وضعیت فعلی (نه تاریخچه) → `IMPLEMENTATION-STATUS.md`

---

## Initial Setup (پیش از ثبت تاریخ مشخص)

Completed:
- [x] Initial solution created
- [x] Server project structure created
- [x] FastEndpoints configured
- [x] Scalar API documentation enabled
- [x] Health endpoint implemented
- [x] Entity Framework Core configured
- [x] PostgreSQL provider configured
- [x] Initial DbContext created
- [x] Initial migration created
- [x] Unit test project created
- [x] Integration test project created
- [x] Health endpoint integration test created
- [x] Git repository initialized
- [x] Initial commit created
- [x] Central package management started

---

## 2026-08-05 — Central Package Management

Completed:
- Central Package Management enabled.
- `Directory.Packages.props` created.
- Package versions moved from individual csproj files.
- All projects updated to reference centralized package versions.
- Clean restore completed.
- Solution build succeeded.
- All unit and integration tests passed.

Next phase: **Domain Modeling v1** — design relational model for installations, hardware info, application versions, telemetry sessions/events, metrics, logs.

Design principles carried forward:
- Avoid duplicate data.
- Keep query performance in mind.
- Store searchable fields as relational columns.
- Store flexible payloads as JSON only when necessary.
- Maintain correlation between logs and telemetry using shared identifiers.

---

## 2026-08-05 — Product Domain EF Integration

Implemented initial Product domain persistence preparation.

- Added Product persistence support into `HJDbContext`.
- Added DbSet registrations: `Product`, `ProductVersion`, `Tenant`.

**Central Package Management — EF Core alignment finalized:**
- Microsoft.EntityFrameworkCore: 10.0.10
- Microsoft.EntityFrameworkCore.Relational: 10.0.10
- Microsoft.EntityFrameworkCore.Design: 10.0.10
- Npgsql.EntityFrameworkCore.PostgreSQL: 10.0.3

Resolved: EF Core Relational version conflict warning, IntegrationTests dependency mismatch, duplicate PackageReference entries.

Status: `dotnet build` PASS, `dotnet test` PASS, no warnings.

---

## 2026-08-05 — Product Persistence Migration

Migration created: `20260805012316_AddProductManagement`

Tables created: `Products`, `ProductVersions`, `Tenants` (fields/constraints documented in `DOMAIN-MODEL.md` and `IMPLEMENTATION-STATUS.md`).

Cleanup: removed empty migration `20260805014406_InitialProductManagement` (accidentally generated, no schema changes).

Status: Build PASS, Tests PASS, Database Layer READY.

---

## 2026-08-05 — Product API Endpoint

Implemented `POST /api/products` via `CreateProductEndpoint` (FastEndpoints).

Flow: `CreateProductRequest → IProductService → IProductRepository → ProductRepository → HJDbContext → Database`

Components completed this session: Domain entity, EF configuration, Repository, Application service + validator, Contracts (DTOs/Request), API endpoint, initial unit tests (create success, duplicate prevention).

---

## 2026-08-05 — Architecture Stabilization

Completed:
- Central Package Management finalized.
- Product Domain implemented.
- Product persistence completed.
- Product API Create endpoint completed.
- Architecture Tests added.
- Unit Tests passed. Integration Tests passed.
- Clean Architecture dependency rules verified.

**Decision point:** development strategy changed from feature-by-feature to domain-first stabilization (see `ADR-001` and `HJPlatform-Development-Context`). Next phase: Installation Management, to be implemented as the first full validation of the stabilized baseline.

---

## 2026-08-06 — Documentation Restructuring

Split the original four context documents into a cleaner structure to avoid duplication and stale-status drift:

- `ARCHITECTURE-RULES.md` created (new, agent-facing, condensed operational ruleset).
- `PROJECT-CONTEXT.md` reduced to static vision/tech-stack content only.
- `PROGRESS-LOG.md` (this file) created to hold chronological entries separately.
- `DOMAIN-MODEL.md` kept as the sole domain reference; implementation-status content removed from it.
- `IMPLEMENTATION-STATUS.md` created as the single current-state snapshot, merging status content previously duplicated across multiple files.

## 2026-08-06 — Installation Feature: Domain Spec

`SPEC-Installation-Domain.md` نوشته شد — اولین مرحله از پایپ‌لاین برای فیچر Installation Management.

پوشش: `Installation` (Aggregate Root) + `InstallationEnvironment` (Entity داخلی) + `IInstallationRepository` + Domain Exceptions.

وضعیت: منتظر کد از ایجنت کدنویس، سپس Review.

`IMPLEMENTATION-STATUS.md` بعد از دریافت و تایید کد آپدیت می‌شود (فعلاً چیزی در کد تغییر نکرده، فقط Spec آماده شده).

## 2026-08-06 — تصمیم: فرمت تحویل کد (Manifest + apply-files.ps1)

چون از ایجنت یکپارچه‌ی IDE استفاده نمی‌شود، روش تحویل کد رسمی شد:

- `scripts/apply-files.ps1` ساخته شد — اسکریپت عمومی و ثابت (یک‌بار نوشته شده، برای همه‌ی فیچرها استفاده می‌شود).
- مدل‌های کدنویس خروجی را در فرمت Manifest (`===FILE: ... ===ENDFILE===`) می‌دهند، نه اسکریپت PowerShell دستی — برای جلوگیری از خطای escaping در here-string های PowerShell.
- این قانون به `ARCHITECTURE-RULES.md` بخش ۱۵ اضافه شد و به `SPEC-Installation-Domain.md` هم رجوع داده شد.
- پیش‌فرض اسکریپت: فایل موجود را overwrite نمی‌کند مگر با `-Force`.

## 2026-08-06 — Installation Domain Layer: تایید نهایی

`dotnet build` و `dotnet test` روی کل سولوشن PASS شد. Domain layer فیچر Installation (`Installation`, `InstallationEnvironment`, `IInstallationRepository`, Exceptions, Unit Tests) طبق `SPEC-Installation-Domain.md` تایید نهایی شد.

**نکته‌ی مهم:** طبق گزارشی که کاربر ارائه داد (بعد از دیباگ با کمک Gemini)، فایل‌های `InstallationConfiguration` و `InstallationEnvironmentConfiguration` در لایه Infrastructure هم تغییر کرده‌اند — **بدون اینکه Spec رسمی لایه Infrastructure نوشته یا کد آن Review شده باشد.** کد واقعی این تغییرات درخواست شد. تا دریافت و Review، این بخش «تایید‌نشده» است.

Next: دریافت کد Infrastructure فعلی → Review → یا تایید یا اصلاح → سپس Spec رسمی Infrastructure (اگر لازم شد).

<!-- ورودی بعدی را اینجا اضافه کن، جدیدترین‌ها پایین -->

---

## 2026-08-07 — Application / Infrastructure / API Completion Review

Completed:

- Application layer review completed.
- API endpoints reviewed.
- Infrastructure persistence reviewed.
- Exception handling flow verified.
- Documentation synchronized.

Validation:

- dotnet build PASS
- dotnet test PASS

