# HJPlatform — Implementation Status

> این فایل تنها منبع «الان دقیقاً چی ساخته شده» است. طراحی هدف در `DOMAIN-MODEL.md` و `ARCHITECTURE-BASELINE.md` است؛ این فایل نشون می‌ده از اون هدف چقدر پیاده شده.
> **این فایل باید مرتب آپدیت بشه** — هر بار یک فیچر کامل می‌شه، اینجا اصلاح کن.
> تاریخچه‌ی روند رسیدن به این وضعیت → `PROGRESS-LOG.md`

**Last updated:** 2026-08-06

---

## خلاصه‌ی وضعیت کلی

| حوزه | وضعیت |
|---|---|
| Infrastructure پایه (solution, FastEndpoints, EF Core, PostgreSQL, testing) | ✅ کامل |
| Product Aggregate | ✅ کامل (Domain تا API، بدون CRUD کامل) |
| Tenant | ✅ Persistence، بدون Application/API مستقل |
| Installation Aggregate 🟢 Domain و Infrastructure اولیه کامل؛ Application/API هنوز پیاده نشده
| Operation Aggregate | ❌ پیاده نشده |
| TelemetryEvent | ❌ پیاده نشده |
| ApplicationLog | ❌ پیاده نشده |
| Notification | ❌ پیاده نشده |
| SDK | ❌ پیاده نشده |
| Architecture Tests | ✅ فعال و پاس |

---

## Product Module (Reference Implementation)

این ماژول به‌عنوان الگوی مرجع پیاده‌سازی برای فیچرهای بعدی استفاده می‌شود.

**Domain:**
- `Product` Entity — `HJ.Server.Domain.Products.Product`
- `ProductVersion` Entity — `HJ.Server.Domain.Products.ProductVersion`
- `IProductRepository`

**Infrastructure:**
- `HJDbContext` — شامل DbSet برای `Product`, `ProductVersion`, `Tenant`
- EF Configurations: `ProductConfiguration`, `ProductVersionConfiguration`, `TenantConfiguration`
  محل: `HJ.Server.Infrastructure.Persistence.Configurations`
- `ProductRepository` (پیاده‌سازی `IProductRepository`)
- DI registration انجام شده

**Application:**
- `IProductService` / `ProductService`
- FluentValidation validator برای Create

**Contracts:**
- `ProductDto`, `ProductVersionDto`, `CreateProductRequest`

**API:**
- `POST /api/products` → `HJ.Server.Api.Endpoints.Products.CreateProductEndpoint`

**Tests:**
- Unit: create product successfully, duplicate product prevention
- Integration: زیرساخت آماده (Health endpoint تست شده؛ Product integration test هنوز کامل اضافه نشده — طبق لاگ، در Next Steps بود)

**Migration:** `20260805012316_AddProductManagement`

### Database Tables (فعلاً موجود)

**Products**
`Id, Code, Name, Description, IsActive, TenantId, CreatedAt, ModifiedAt`
Constraint: Unique `(TenantId, Code)`

**ProductVersions**
`Id, ProductId, Version, BuildNumber, ReleaseNotes, ReleaseDate, Status, UpdatePolicy, TenantId, CreatedAt, ModifiedAt`
Constraint: Unique `(ProductId, Version)`

**Tenants**
`Id, Name, Code, IsActive, TenantId, CreatedAt, ModifiedAt`
Constraint: Unique `Code`

> نکته: `VersionStatus` و `UpdatePolicy` به‌صورت Enum در کد وجود دارند؛ مقادیر دقیق باید از سورس‌کد خوانده شود، نه حدس زده شود.

---

## Installation Module — وضعیت تفصیلی

**Domain (✅ Review‌شده و تایید‌شده — 2026-08-06):**
- `Installation` (Aggregate Root) — ارث از `BaseEntity`، تمام property ها `private set`، `ProductId` غیرقابل‌تغییر بعد از ساخت.
- `InstallationEnvironment` (Entity داخلی، بدون Repository مستقل) — FK صحیح به `Installation.Id`.
- `IInstallationRepository` — ۴ متد طبق Spec.
- `InstallationAlreadyExistsException`, `InstallationNotFoundException`.
- Unit Tests (`InstallationTests.cs`, xUnit) — ۵ سناریو، پاس.
- `dotnet build` و `dotnet test` روی کل سولوشن: PASS.

**Infrastructure (🟢 پیاده‌سازی شده):**
طبق گزارش کاربر (از طریق Gemini)، `InstallationConfiguration` و `InstallationEnvironmentConfiguration` در EF Core آپدیت شده‌اند. **این کار بدون Spec رسمی لایه Infrastructure انجام شده و کد واقعی‌اش هنوز برای Review دریافت نشده.** تا وقتی کد بررسی نشه، این بخش را «تایید‌شده» در نظر نگیر.

- InstallationConfiguration
- InstallationEnvironmentConfiguration
- EF Migration:
  20260806135022_UpdateInstallationModel

**باقی‌مانده:**
- Migration رسمی برای جداول Installation / InstallationEnvironment (وضعیت نامعلوم — باید چک شود آیا قبلاً ساخته شده)
- Repository Implementation (`InstallationRepository`)
- Application Service
- Contracts / DTO
- API Endpoint

Processing Module

ProcessingJob در بازبینی معماری V1 حذف شد.

دلایل:
- مفهوم مستقل دامنه‌ای ایجاد نمی‌کرد.
- مسئولیت آن با Operation Aggregate هم‌پوشانی داشت.
- Tracking اجرای پردازش‌ها باید از طریق Operation و OperationExecution انجام شود.

وضعیت:
- ProcessingJob Entity حذف شد.
- ProcessingJobConfiguration حذف شد.
- Migration حذف Entity ایجاد شد.

### Operation / TelemetryEvent / ApplicationLog
فقط در سطح طراحی دامنه مستند شده‌اند (`DOMAIN-MODEL.md`). هیچ کدی نوشته نشده.

### Notification

در Baseline معماری اولیه مطرح شده بود، اما در Domain Model V1 فعلی Aggregate مستقلی ندارد.

وضعیت:
- طراحی دامنه انجام نشده.
- Entity یا Repository ندارد.
- نیازمند تصمیم معماری قبل از پیاده‌سازی است.

### SDK
هیچ کاری روی SDK شروع نشده.

---

## Architecture Validation

- Architecture Tests اضافه شده‌اند و قوانین dependency بین لایه‌ها را چک می‌کنند.
- تمام تست‌ها (Unit + Integration + Architecture) در آخرین اجرا PASS بوده‌اند.
- Warning شناخته‌شده‌ای در build وجود ندارد.

---

## نحوه‌ی آپدیت این فایل

بعد از تکمیل هر فیچر (طبق پایپ‌لاین ۱۱ مرحله‌ای در `ARCHITECTURE-RULES.md`):
1. جدول "خلاصه‌ی وضعیت کلی" را آپدیت کن.
2. یک بخش جدید برای ماژول تکمیل‌شده اضافه کن (مثل بخش Product Module بالا).
3. بخش‌های "Not Yet Implemented" را متناسب کوچک کن.
4. تاریخ "Last updated" را عوض کن.
5. یک ورودی متناظر هم در `PROGRESS-LOG.md` ثبت کن (آن فایل تاریخچه است، این فایل عکس لحظه‌ای است).
