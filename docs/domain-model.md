# HJPlatform Domain Model (V1)

> **Status:** Stable (V1) — Baseline
> **Last Updated:** 2026-08-05
> این سند فقط طراحی دامنه است. برای وضعیت فعلی پیاده‌سازی → `IMPLEMENTATION-STATUS.md`
> تغییر ساختاری در این سند فقط از طریق ADR مجاز است.

---

# هدف

این سند مدل دامنه (Domain Model) نسخه اول HJPlatform را تعریف می‌کند.

تمام Entityها، ارتباطات، مسئولیت‌ها و قوانین این سند مرجع اصلی طراحی دامنه هستند و تغییر در آن‌ها تنها از طریق تصمیم معماری (ADR) انجام می‌شود.

---

# معماری

معماری پروژه بر پایه مفاهیم زیر طراحی شده است:

* Domain Driven Design (DDD)
* Clean Architecture
* Aggregate Root
* Repository Pattern
* CQRS Ready
* Multi-Tenant Ready

---

# Aggregateها

سیستم دارای چهار Aggregate اصلی است.

```
Tenant

Product
    └── ProductVersion

Installation
    └── InstallationEnvironment

Operation
    ├── OperationExecution
    ├── TelemetryEvent
    └── ApplicationLog
```

---

# Entityها

---

# Tenant

نماینده یک سازمان یا مشتری مستقل در سیستم.

## مسئولیت

* مالک داده‌ها
* جداسازی اطلاعات
* پشتیبانی از Multi-Tenant

## Properties

| Property   | Type      | Description     |
| ---------- | --------- | --------------- |
| Id         | Guid      | شناسه           |
| Name       | string    | نام Tenant      |
| Code       | string    | کد یکتا         |
| IsActive   | bool      | فعال یا غیرفعال |
| CreatedAt  | DateTime  | زمان ایجاد      |
| ModifiedAt | DateTime? | آخرین تغییر     |

---

# Product

نماینده یک نرم‌افزار.

نمونه‌ها:

* HJ Image Optimizer
* HJ CAD Plugin
* HJ PDF Tools

## مسئولیت

* تعریف محصول
* نگهداری نسخه‌ها

## Properties

| Property    | Type      |
| ----------- | --------- |
| Id          | Guid      |
| TenantId    | Guid?     |
| Code        | string    |
| Name        | string    |
| Description | string?   |
| IsActive    | bool      |
| CreatedAt   | DateTime  |
| ModifiedAt  | DateTime? |

## Aggregate

```
Product
    └── ProductVersion
```

---

# ProductVersion

نماینده یک نسخه منتشر شده از محصول.

نمونه:

```
1.0.0

1.1.2

2.0.0
```

## مسئولیت

* مدیریت انتشار نسخه
* سیاست آپدیت
* وضعیت انتشار

## Properties

| Property     | Type          |
| ------------ | ------------- |
| Id           | Guid          |
| ProductId    | Guid          |
| Version      | string        |
| BuildNumber  | string?       |
| ReleaseNotes | string?       |
| ReleaseDate  | DateTime      |
| Status       | VersionStatus |
| UpdatePolicy | UpdatePolicy  |
| CreatedAt    | DateTime      |
| ModifiedAt   | DateTime?     |

---

# Installation

نماینده نصب یک محصول روی یک دستگاه.

هر نصب دارای شناسه یکتای دائمی است.

## مسئولیت

* شناسایی نصب
* نگهداری نسخه نصب‌شده
* ارتباط با عملیات اجرا شده

## Properties

| Property         | Type     |
| ---------------- | -------- |
| Id               | Guid     |
| InstallationId   | Guid     |
| ProductId        | Guid     |
| ProductVersionId | Guid     |
| FirstSeenAt      | DateTime |
| LastSeenAt       | DateTime |

---

# InstallationEnvironment

اطلاعات سخت‌افزار و سیستم عامل هنگام نصب.

این اطلاعات فقط جهت تحلیل و عیب‌یابی استفاده می‌شوند.

## مسئولیت

* مشخصات سیستم
* تحلیل محیط اجرا

## Properties

| Property           | Type     |
| ------------------ | -------- |
| Id                 | Guid     |
| InstallationId     | Guid     |
| OSVersion          | string?  |
| CpuName            | string?  |
| CpuCoreCount       | int      |
| RamGB              | double   |
| ScreenResolution   | string?  |
| HardwareIdentifier | string?  |
| CreatedAt          | DateTime |

---

# Operation

نماینده یک عملیات سطح بالا که توسط نرم‌افزار انجام شده است.

نمونه‌ها:

* Optimize Images
* Export DWG
* Convert PDF
* AI Analyze
* Check Update

## مسئولیت

* ثبت شروع و پایان عملیات
* ایجاد Correlation بین Telemetry و Log

## Properties

| Property       | Type      |
| -------------- | --------- |
| Id             | Guid      |
| InstallationId | Guid      |
| CorrelationId  | Guid      |
| Type           | string    |
| StartedAt      | DateTime  |
| EndedAt        | DateTime? |
| Status         | string    |

---

# OperationExecution

نماینده نتیجه اجرای یک Operation.

هر Operation می‌تواند اطلاعات آماری اجرای خود را در این Entity ثبت کند.

## مسئولیت

* آمار اجرا
* اطلاعات عملکرد

## Properties

| Property         | Type     |
| ---------------- | -------- |
| Id               | Guid     |
| OperationId      | Guid     |
| ExecutionSource  | string   |
| ExecutionMode    | string?  |
| ItemsCount       | int      |
| SucceededCount   | int      |
| FailedCount      | int      |
| ConcurrencyLevel | int      |
| DurationMs       | long     |
| MetadataJson     | string?  |
| CreatedAt        | DateTime |

---

# TelemetryEvent

رویدادهای ارسالی از کلاینت.

نمونه‌ها:

```
ImageOptimized

UpdateChecked

ExportFinished

LicenseValidated
```

## مسئولیت

* ثبت Eventها
* تحلیل رفتار کاربران
* گزارش استفاده

## Properties

| Property       | Type     |
| -------------- | -------- |
| Id             | Guid     |
| InstallationId | Guid     |
| OperationId    | Guid     |
| EventName      | string   |
| EventVersion   | int      |
| PayloadJson    | string   |
| CreatedAt      | DateTime |

---

# ApplicationLog

ثبت Logهای سمت کلاینت.

نمونه‌ها:

* Error
* Warning
* Information

## مسئولیت

* خطاها
* Exceptionها
* Debug اطلاعات

## Properties

| Property       | Type     |
| -------------- | -------- |
| Id             | Guid     |
| InstallationId | Guid     |
| OperationId    | Guid     |
| Level          | string   |
| Message        | string   |
| ExceptionJson  | string?  |
| PropertiesJson | string?  |
| CreatedAt      | DateTime |

---

# Enumها

## VersionStatus

```
Draft

Published

Deprecated

Retired
```

---

## UpdatePolicy

```
Optional

Recommended

Required

Blocked
```

---

# روابط بین Entityها

```
Tenant
    │
    │ 1
    │
    └───────────────*
                    │
                Product
                    │
                    │ 1
                    │
                    └───────────────*
                                    │
                             ProductVersion

Product
    │
    │ 1
    │
    └───────────────*
                    │
              Installation
                    │
                    │ 1
                    │
                    └──────────────1
                                    │
                      InstallationEnvironment

Installation
      │
      │ 1
      │
      └───────────────*
                      │
                  Operation
        ┌─────────────┼─────────────┐
        │             │             │
        │             │             │
        *             *             *
        │             │             │
OperationExecution TelemetryEvent ApplicationLog
```

---

# قوانین دامنه

## Product

* Code باید یکتا باشد.
* Name اجباری است.
* حذف Product باعث حذف Versionها می‌شود.

---

## ProductVersion

* Version در هر Product یکتا است.
* هر Version دقیقاً متعلق به یک Product است.

---

## Installation

* InstallationId یکتا است.
* هر Installation متعلق به یک Product است.
* هر Installation فقط یک نسخه فعال دارد.

---

## Operation

* همیشه متعلق به یک Installation است.
* پایان عملیات می‌تواند Null باشد.
* CorrelationId برای ارتباط تمامی داده‌های مرتبط استفاده می‌شود.

---

## Telemetry

* همیشه به یک Installation تعلق دارد.
* در صورت وجود Operation، به آن متصل می‌شود.
* PayloadJson باید نسخه‌بندی شود.

---

## Logging

* Logها فقط جهت تحلیل و Debug استفاده می‌شوند.
* حذف Log نباید روی سایر Entityها اثری داشته باشد.

---

# اصول طراحی

* Domain مستقل از EF Core است.
* Domain مستقل از API است.
* Domain مستقل از Database است.
* Repository فقط در Domain تعریف می‌شود.
* Infrastructure تنها محل پیاده‌سازی Repositoryها است.
* تمام Aggregateها دارای Root مشخص هستند.
* هیچ Entity خارج از Aggregate مستقیماً Entity داخلی Aggregate دیگر را تغییر نمی‌دهد.

---

# وضعیت مدل

این مدل به عنوان **Baseline Domain Model (V1)** پذیرفته شده و هرگونه تغییر ساختاری در Entityها، روابط یا Aggregateها باید از طریق مستندات Architecture Decision Record (ADR) انجام شود.

> **مهم:** وضعیت پیاده‌سازی (چه چیزی الان واقعاً در کد وجود دارد) در این سند نگهداری نمی‌شود — این سند فقط طراحی هدف را نشان می‌دهد. برای وضعیت فعلی → `IMPLEMENTATION-STATUS.md`.
