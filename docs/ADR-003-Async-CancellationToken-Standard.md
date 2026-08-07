# ADR-003: Async CancellationToken Standard

## Status
Accepted

## Context

عملیات Async در لایه‌های مختلف پروژه وجود دارد و برای مدیریت صحیح درخواست‌های لغو شده باید رفتار یکسانی داشته باشند.

## Decision

تمام متدهای Async عمومی در Application و Repository باید پارامتر زیر را داشته باشند:

```csharp
CancellationToken cancellationToken = default

این Token باید تا پایین‌ترین لایه ممکن منتقل شود.

Consequences

مزایا:

مدیریت بهتر منابع
سازگاری با ASP.NET Core Request Pipeline
جلوگیری از عملیات غیرضروری بعد از لغو درخواست
Scope

تمام سرویس‌ها، Repositoryها و عملیات Persistence جدید.