# HJ.Server Architecture Baseline v1.0

> **Status:** Approved Baseline
> **Version:** 1.0
> **Architecture:** Clean Architecture + DDD + Vertical Slice
> این سند نسخه‌ی کامل با rationale برای انسان‌هاست. نسخه‌ی فشرده و عملیاتی برای ایجنت‌های کدنویس → `ARCHITECTURE-RULES.md`
> برای وضعیت فعلی پیاده‌سازی → `IMPLEMENTATION-STATUS.md`

---

# 1. هدف

HJ.Server زیرساخت مرکزی تمامی محصولات HJ است.

این سرور وظیفه ارائه سرویس‌های عمومی و مشترک برای تمام محصولات فعلی و آینده را بر عهده دارد و نباید وابسته به منطق تجاری یک نرم‌افزار خاص باشد.

---

# 2. اهداف سیستم

## قابلیت‌های فعلی

- Product Management
- Release Management
- Update Service
- Installation Management
- Telemetry
- Logging
- Notification

## قابلیت‌های آینده

- Licensing
- Authentication
- Billing
- Plugin Marketplace
- Analytics
- User Management

---

# 3. اهداف غیرمجاز

HJ.Server نباید شامل منطق اختصاصی یک محصول باشد.

نمونه‌های نادرست:

- ImageOptimizerBatch
- AutoCADDrawing
- PdfCompression
- ExcelProcessor

تمام اطلاعات اختصاصی محصولات باید به صورت داده (Payload) یا Event ذخیره شوند، نه به صورت Entity اختصاصی.

---

# 4. معماری کلان

```text
             Client
                │
                ▼
              SDK
                │
                ▼
              API
                │
                ▼
          Application
                │
                ▼
             Domain
                │
                ▼
         Infrastructure
                │
                ▼
            PostgreSQL
```

وابستگی‌ها همیشه فقط به سمت داخل هستند.

---

# 5. مسئولیت لایه‌ها

## Domain

شامل:

- Entity
- Aggregate
- Aggregate Root
- Value Object
- Domain Service
- Repository Interface
- Business Rule
- Enum
- Domain Event

نباید هیچ وابستگی به موارد زیر داشته باشد:

- EF Core
- ASP.NET Core
- FastEndpoints
- PostgreSQL
- Serilog
- Http

---

## Application

مسئول اجرای Use Caseها است.

نمونه:

- CreateProduct
- PublishRelease
- RegisterInstallation
- CheckForUpdate
- UploadTelemetry
- UploadLog
- SendNotification

Application فقط با Repository Interface کار می‌کند.

---

## Infrastructure

شامل:

- EF Core
- Repository Implementation
- Database
- Cache
- File Storage
- External Services
- Email
- Queue

---

## API

مسئول:

- HTTP
- Authentication
- Authorization
- Validation
- Mapping
- Response

هیچ Business Rule نباید داخل Endpoint قرار بگیرد.

---

## SDK

SDK تنها راه ارتباط Client با Server است.

تمام جزئیات:

- HTTP
- Serialization
- Retry
- Authentication
- Compression

باید داخل SDK مخفی شوند.

---

# 6. قوانین Domain

Entity صرفاً Data Container نیست.

Entity باید رفتار (Behavior) داشته باشد.

نمونه صحیح:

```csharp
product.Disable();

release.Publish();

tenant.Activate();
```

نمونه نادرست:

```csharp
product.IsActive = false;
```

---

# 7. Aggregateها

هر Aggregate فقط یک Root دارد.

در نسخه فعلی:

- Product
- Tenant
- Installation
- Operation
- Notification

Release زیرمجموعه Product محسوب می‌شود و Aggregate مستقل نیست.

---

# 8. Repository

Repository فقط برای Aggregate Root نوشته می‌شود.

نمونه:

```text
IProductRepository

ITenantRepository

IInstallationRepository

IOperationRepository

INotificationRepository
```

برای Release Repository مستقل وجود ندارد.

---

# 9. DTO

تمام DTOها داخل پروژه Contracts قرار می‌گیرند.

Domain هیچ اطلاعی از DTO ندارد.

---

# 10. Mapping

تمام Mappingها در Application انجام می‌شوند.

Domain نباید DTO تولید کند.

---

# 11. Validation

Validation در سه سطح انجام می‌شود.

## Request Validation

- Required
- Length
- Regex
- Email
- Semantic Version Format

---

## Domain Validation

Business Rule

نمونه:

- Duplicate Product Code
- Duplicate Version
- Publish روی Release حذف‌شده ممنوع
- Tenant غیرفعال

---

## Database Validation

- Unique Index
- Foreign Key
- Check Constraint

---

# 12. Exception

Business Ruleها Exception عمومی پرتاب نمی‌کنند.

نمونه:

```text
ProductAlreadyExistsException

TenantDisabledException

ReleaseAlreadyPublishedException
```

---

# 13. Result Pattern

Application ترجیحاً از

```csharp
Result<T>
```

استفاده می‌کند.

Exception فقط برای خطاهای غیرمنتظره است.

---

# 14. شناسه‌ها

تمام Entityها دارای

```text
Guid
```

هستند.

Identity عددی استفاده نمی‌شود.

---

# 15. زمان

تمام تاریخ‌ها و زمان‌ها به صورت

```text
UTC
```

ذخیره می‌شوند.

تبدیل به Local Time فقط در Client انجام می‌شود.

---

# 16. Multi-Tenant

تمام Entityهای Tenant-Aware دارای

```text
TenantId
```

هستند.

تمام Queryها باید Tenant را فیلتر کنند.

---

# 17. Soft Delete

در نسخه فعلی استفاده نمی‌شود.

حذف‌ها به صورت Hard Delete انجام می‌شوند.

---

# 18. Versioning

نسخه‌ها بر اساس Semantic Version ذخیره می‌شوند.

نمونه:

```text
1.0.0

1.2.5

2.0.0-preview1
```

---

# 19. Logging

Log فقط برای:

- Debug
- Information
- Warning
- Error

استفاده می‌شود.

اطلاعات تحلیلی داخل Log ذخیره نمی‌شوند.

---

# 20. Telemetry

Telemetry برای تحلیل رفتار سیستم است.

ساختار Event:

```text
EventName

EventVersion

PayloadJson
```

Payload ساختار ثابت ندارد.

---

# 21. Operation

هر عملیات کاربر دارای یک OperationId است.

تمام موارد زیر به آن متصل می‌شوند:

- Log
- Telemetry
- Diagnostics
- Performance

---

# 22. Notification

Notification مستقل از محصول است.

می‌تواند برای:

- همه کاربران
- یک Product
- یک Version
- یک Tenant
- یک Installation

ارسال شود.

---

# 23. Release Management

هر Product

دارای چندین Release است.

تنها Releaseهای Published قابل ارائه به کاربران هستند.

Update Policy تعیین می‌کند:

- Optional
- Recommended
- Required
- Blocked

---

# 24. API Design

Endpointها باید Resource-Oriented باشند.

نمونه:

```text
GET     /products

GET     /products/{id}

POST    /products

PUT     /products/{id}

DELETE  /products/{id}
```

Endpointهایی مانند

```text
/DoSomething
```

یا

```text
/Execute
```

مجاز نیستند.

---

# 25. Database Rules

- تمام Foreign Keyها صریح تعریف می‌شوند.
- تمام Indexها مستند هستند.
- Cascade Delete فقط در صورت نیاز استفاده می‌شود.
- Naming تمام جداول و ستون‌ها یکنواخت است.

---

# 26. Migration Rules

هر Feature مستقل Migration خود را دارد.

Migrationهای منتشر شده هرگز ویرایش نمی‌شوند.

---

# 27. Naming Convention

## Entity

```text
Product

ProductRelease

Tenant

Installation

Operation

Notification
```

## Repository

```text
IProductRepository
```

## DTO

```text
ProductDto
```

## Request

```text
CreateProductRequest
```

## Response

```text
CheckForUpdateResponse
```

## Endpoint

```text
CreateProductEndpoint
```

---

# 28. Dependency Rule

```text
API
 │
 ▼
Application
 │
 ▼
Domain

Infrastructure ─────► Domain
```

Domain به هیچ پروژه‌ای وابسته نیست.

---

# 29. ترتیب توسعه Feature جدید

برای هر قابلیت جدید ترتیب توسعه ثابت است.

1. Domain
2. Repository Interface
3. Infrastructure
4. Application
5. Contracts
6. API
7. SDK
8. Tests

عبور از این ترتیب مجاز نیست.

---

# 30. اصول توسعه

- هر Entity فقط یک مسئولیت دارد.
- هر Aggregate فقط یک Root دارد.
- Business Ruleها فقط در Domain پیاده‌سازی می‌شوند.
- API فاقد منطق تجاری است.
- Payloadهای پویا به صورت JSON ذخیره می‌شوند.
- مدل رابطه‌ای بر JSON اولویت دارد.
- تمام قابلیت‌های جدید باید بدون شکستن قراردادهای موجود اضافه شوند.

---

# 31. وضعیت سند

این سند به عنوان **Architecture Baseline v1.0** مبنای رسمی توسعه HJ.Server محسوب می‌شود.

تمام توسعه‌های بعدی باید با این سند سازگار باشند.

هر تغییری که باعث تغییر قوانین این سند شود، باید با افزایش نسخه سند (v1.1، v2.0 و ...) انجام شود و نباید به صورت موردی در حین توسعه اعمال گردد.

---

**Approved:** HJ.Server Architecture Baseline v1.0

> وضعیت پیاده‌سازی این Baseline (چه چیزی الان واقعاً ساخته شده) در این سند نگهداری نمی‌شود. برای آن → `IMPLEMENTATION-STATUS.md`.
