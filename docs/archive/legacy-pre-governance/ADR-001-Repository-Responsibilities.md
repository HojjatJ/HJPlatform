# ADR-001: Repository Responsibilities

## Status
Accepted

## Context

در نسخه اولیه HJPlatform، رفتار Repositoryها یکسان نبود.

برخی Repositoryها مانند ProductRepository در متدهای AddAsync عملیات SaveChanges را انجام می‌دادند، در حالی که برخی دیگر مانند InstallationRepository این مسئولیت را به Application Service واگذار می‌کردند.

این تفاوت باعث ایجاد رفتار غیرقابل پیش‌بینی در Transaction Boundary می‌شد.

## Decision

Repository فقط مسئول عملیات Persistence است:

- Add
- Update
- Remove
- Query

Repository نباید عملیات Commit یا SaveChanges انجام دهد.

مدیریت Commit در لایه بالاتر انجام خواهد شد.

## Consequences

مزایا:

- کنترل بهتر Transaction
- امکان ترکیب چند تغییر در یک عملیات
- رفتار یکسان بین ماژول‌ها

محدودیت:

- نیاز به تعریف UnitOfWork در صورت پیچیده شدن Transactionها در آینده

## Scope

این تصمیم برای تمام Repositoryهای HJPlatform اعمال می‌شود.