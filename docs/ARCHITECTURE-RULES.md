# HJPlatform — Architecture Rules (Agent-Facing)

> این سند برای دادن به هر ایجنت کدنویس (Cursor, Copilot, ChatGPT, Gemini, ...) قبل از هر تسک نوشتن کد استفاده می‌شود.
> منبع این قوانین: `ARCHITECTURE-BASELINE.md` و `DOMAIN-MODEL.md`. در صورت تناقض، آن دو سند مرجع نهایی‌اند.
> **این سند فقط قوانین است، نه Spec فیچر. Spec فیچر جداگانه داده می‌شود.**

---

## 0. قانون طلایی

اگر اجرای یک تسک نیاز به نقض یکی از قوانین این سند دارد:
**متوقف شو و بپرس. حدس نزن. خودسرانه تصمیم معماری نگیر.**

هرگونه تغییر ساختاری در Aggregate ها، روابط Entity، یا قوانین لایه‌بندی نیازمند ADR است و بدون تایید صریح انسان مجاز نیست.

---

## 1. Tech Stack (ثابت — تغییر ندهید)

| لایه | تکنولوژی |
|---|---|
| Framework | ASP.NET Core (.NET 10) |
| API | FastEndpoints |
| API Docs | Scalar.AspNetCore |
| Validation | FluentValidation |
| Mapping | Riok.Mapperly (compile-time، **نه** AutoMapper، **نه** reflection-based) |
| ORM | Entity Framework Core |
| Database | PostgreSQL (Npgsql.EntityFrameworkCore.PostgreSQL) |
| Logging | Serilog |
| Unit Tests | xUnit |
| Integration Tests | Microsoft.AspNetCore.Mvc.Testing |
| DB Tests | Testcontainers.PostgreSql |
| Package Management | Central (`Directory.Packages.props`) — هرگز Version را داخل csproj ننویس |

---

## 2. ساختار پروژه (تغییر نده، فقط داخلش فایل اضافه کن)

```
src/
  HJ.Server.Api             → HTTP, Endpoints, wiring
  HJ.Server.Application     → Use Cases, orchestration
  HJ.Server.Domain          → Entities, Aggregates, Business Rules
  HJ.Server.Infrastructure  → EF Core, Repositories, external services
  HJ.Server.Contracts       → DTOs, Request/Response models
  HJ.Server.Foundation      → Shared primitives (BaseEntity, common utils)

tests/
  HJ.Server.UnitTests
  HJ.Server.IntegrationTests
```

---

## 3. Dependency Rule (سخت‌گیرانه)

```
API → Application → Domain
Infrastructure → Domain
```

**Domain هیچ وابستگی‌ای به موارد زیر ندارد:**
- EF Core
- ASP.NET Core / FastEndpoints
- PostgreSQL / Npgsql
- Serilog
- هیچ چیز مربوط به HTTP

اگر کدی که تولید می‌کنی در `HJ.Server.Domain` نیاز به `using Microsoft.EntityFrameworkCore` یا مشابه دارد → **اشتباه است، متوقف شو.**

پروژه Architecture Tests وجود دارد و این قوانین را به‌صورت خودکار چک می‌کند — کد باید این تست‌ها را پاس کند.

---

## 4. قوانین Domain

### 4.1 Entity رفتار دارد، نه فقط داده

❌ غلط:
```csharp
product.IsActive = false;
```

✅ درست:
```csharp
product.Disable();
```

هر تغییر وضعیت مهم باید یک متد با معنای کسب‌وکار روی Entity باشد، نه ست‌کردن مستقیم property.

### 4.2 Aggregate ها (فعلاً همین ۴ تا — بدون اضافه‌کردن جدید بدون تایید)

```
Tenant
Product        └── ProductVersion
Installation   └── InstallationEnvironment
Operation      ├── OperationExecution
               ├── TelemetryEvent
               └── ApplicationLog
```

- هر Aggregate دقیقاً یک Root دارد.
- هیچ Entity خارج از Aggregate مستقیماً یک Entity داخلی Aggregate دیگر را تغییر نمی‌دهد — فقط از طریق Root.
- برای Entity های غیر-Root (مثل `ProductVersion`, `InstallationEnvironment`) **Repository جدا نساز**. فقط برای Aggregate Root Repository تعریف می‌شود.

### 4.3 شناسه و زمان

- تمام Identity ها `Guid` هستند. **هرگز `int`/`long` auto-increment استفاده نکن.**
- تمام تاریخ‌ها `UTC` ذخیره می‌شوند. تبدیل به Local Time فقط در Client انجام می‌شود، نه در سرور.

### 4.4 Multi-Tenant

- هر Entity که Tenant-Aware است باید فیلد `TenantId` داشته باشد (از `BaseEntity` می‌آید).
- **هر Query باید Tenant را فیلتر کند.** فراموش‌کردن این فیلتر یک باگ امنیتی جدی است، نه یک جزئیات.

### 4.5 Soft Delete

فعلاً استفاده نمی‌شود. حذف = Hard Delete. (این ممکن است در آینده تغییر کند، ولی الان قانون همین است.)

---

## 5. Repository

- فقط برای Aggregate Root نوشته می‌شود: `IProductRepository`, `ITenantRepository`, `IInstallationRepository`, `IOperationRepository`, `INotificationRepository`.
- Interface در Domain تعریف می‌شود، Implementation در Infrastructure.
- Application فقط با Interface کار می‌کند، هرگز مستقیماً با DbContext.

---

## 6. DTO و Mapping

- تمام DTO ها در پروژه `HJ.Server.Contracts` هستند.
- **Domain هیچ اطلاعی از DTO ندارد** — Domain نباید DTO تولید یا مصرف کند.
- Mapping فقط در لایه Application انجام می‌شود، با Mapperly (compile-time).

---

## 7. Validation (سه سطح — هر سه باید رعایت شوند)

1. **Request Validation** (FluentValidation، در API/Contracts): Required, Length, Regex, Format
2. **Domain Validation** (Business Rule، در Domain): مثل Duplicate Code، Tenant غیرفعال — این‌ها Exception اختصاصی پرتاب می‌کنند نه Exception عمومی
3. **Database Validation**: Unique Index, Foreign Key, Check Constraint

---

## 8. Exception و Result Pattern

- Business Rule ها Exception **اختصاصی** پرتاب می‌کنند، نه `Exception` عمومی:
  ```
  ProductAlreadyExistsException
  TenantDisabledException
  ReleaseAlreadyPublishedException
  ```
- لایه Application ترجیحاً از `Result<T>` استفاده می‌کند. Exception فقط برای خطاهای واقعاً غیرمنتظره است.

---

## 9. API Design

- Endpoint ها **Resource-Oriented** هستند:
  ```
  GET    /products
  GET    /products/{id}
  POST   /products
  PUT    /products/{id}
  DELETE /products/{id}
  ```
- Endpoint هایی مثل `/DoSomething` یا `/Execute` **ممنوع**.
- هیچ Business Rule داخل Endpoint نمی‌آید. Endpoint فقط: دریافت Request → صدا زدن Application Service → برگرداندن Response.

---

## 10. Naming Convention

| نوع | الگو | مثال |
|---|---|---|
| Entity | اسم تکی | `Product`, `Installation` |
| Repository | `I{Entity}Repository` | `IProductRepository` |
| DTO | `{Entity}Dto` | `ProductDto` |
| Request | `{Action}{Entity}Request` | `CreateProductRequest` |
| Response | `{Action}Response` | `CheckForUpdateResponse` |
| Endpoint | `{Action}{Entity}Endpoint` | `CreateProductEndpoint` |

---

## 11. Migration

- هر Feature، Migration مستقل خودش را دارد.
- Migration های منتشرشده **هرگز ویرایش نمی‌شوند** — اگر نیاز به تغییر هست، Migration جدید بساز.
- قبل از commit، مطمئن شو Migration خالی یا اضافی تولید نشده (مثل چیزی که در Product module اتفاق افتاد و پاک شد).

---

## 12. پایپ‌لاین اجباری توسعه فیچر جدید

ترتیب زیر برای **هر** فیچر جدید ثابت است. رد کردن یا جابه‌جا کردن مراحل مجاز نیست:

```
1. Domain (Entity + Business Rules)
2. Repository Interface
3. Infrastructure (EF Configuration + Repository Impl)
4. Migration
5. Application (Service + Use Case)
6. Contracts (DTO + Request/Response)
7. API (Endpoint)
8. SDK
9. Tests (Unit + Integration)
10. Documentation
11. Git commit
```

اگر ایجنت کدنویس یک‌باره همه چیز را با هم تولید کند (مثلاً Entity و Endpoint را در یک پاسخ)، این قابل قبول است **فقط اگر ترتیب منطقی و لایه‌بندی رعایت شده باشد** — مهم رعایت Dependency Rule است، نه لزوماً تعداد پیام‌ها.

---

## 13. ممنوعیت‌های صریح

- ❌ ساخت Entity اختصاصی یک محصول خاص (مثل `ImageOptimizerBatch`, `PdfCompression`). اطلاعات اختصاصی محصول باید به‌صورت Payload/Event ذخیره شود، نه Entity.
- ❌ اضافه‌کردن abstraction یا framework جدید داخل پروژه بدون دلیل روشن.
- ❌ اضافه‌کردن Field به Entity بدون توجیه کسب‌وکاری مشخص.
- ❌ نوشتن Repository برای Entity غیر-Root.
- ❌ استفاده از AutoMapper یا هر mapping مبتنی بر reflection.
- ❌ Cascade Delete پیش‌فرض — فقط جایی که واقعاً نیاز است.
- ❌ اضافه‌کردن dependency جدید (NuGet package) بدون بررسی: نگه‌داری، لایسنس، پیچیدگی، تأثیر بلندمدت.

---

## 14. تست

هر فیچر جدید باید شامل:
- Unit Test برای منطق Domain و Application
- Integration Test برای مسیر کامل API → Database
- در صورت نیاز، Architecture Test برای قانون جدید لایه‌بندی

---

## 15. نحوه‌ی تحویل کد (Code Delivery Format)

توسعه‌دهنده از ایجنت یکپارچه‌ی IDE استفاده نمی‌کند. کد از مدل کدنویس (چت) دریافت می‌شود، سپس با یک اسکریپت عمومی (`tools/apply-files.ps1`) روی فایل‌سیستم اعمال می‌شود.

**قانون برای مدل کدنویس:** خروجی کد را همیشه در فرمت زیر بده، نه به شکل بلوک‌های کد جدا-جدا و نه به شکل اسکریپت PowerShell دستی:

```
===FILE: relative/path/to/File1.cs
<کل محتوای فایل، بدون Markdown fence>
===ENDFILE===
===FILE: relative/path/to/File2.cs
<محتوای فایل دوم>
===ENDFILE===
```

قوانین این فرمت:
- مسیر هر فایل **نسبت به ریشه‌ی ریپازیتوری** نوشته شود (مثل `src/HJ.Server.Domain/Installations/Installation.cs`).
- هیچ توضیح یا متن اضافه بیرون از بلوک‌های `===FILE:` / `===ENDFILE===` قرار نگیرد.
- محتوای هر فایل عیناً همان چیزی است که ذخیره می‌شود — بدون سه‌بک‌تیک (```) اضافه.
- اگر خروجی طولانی شد و احتمال بریدگی هست، مدل باید Manifest را به چند بخش منطقی (مثلاً Entity ها جدا، Exception ها جدا) تقسیم کند و هر بخش را با عنوان "بخش ۱ از ۲" مشخص کند.

**نحوه‌ی اعمال (توسط توسعه‌دهنده):**
```powershell
./tools/apply-files.ps1 -ManifestPath .\manifest.txt
```
اگر نیاز به بازنویسی فایل موجود بود:
```powershell
./tools/apply-files.ps1 -ManifestPath .\manifest.txt -Force
```

پیش‌فرض اسکریپت: فایل موجود را **بازنویسی نمی‌کند** (فقط با `-Force` بازنویسی می‌شود) — این یک محافظ در برابر از‌دست‌رفتن تغییرات دستی است.

**بعد از اعمال فایل‌ها، همیشه این دو دستور اجرا شود:**
```powershell
dotnet build
dotnet test
```
اگر build یا test شکست خورد، خروجی خطا برای Review به Claude داده شود؛ حدس زده نشود که مشکل کجاست.

---

## 16. نحوه استفاده از این سند

**برای ایجنت کدنویس:** این فایل را قبل از هر تسک کدنویسی کامل بخوان. اگر Spec فیچر با این قوانین تناقض داشت، تناقض را اعلام کن، پیش‌فرض نگیر.

**برای Review (Claude):** هر کد دریافتی در برابر بخش‌های ۲ تا ۱۴ این سند چک می‌شود، به‌خصوص بخش ۳ (Dependency Rule) و بخش ۱۳ (ممنوعیت‌ها).

---

**وضعیت سند:** فعال، مشتق‌شده از Architecture Baseline v1.0
**تغییر این سند فقط هم‌زمان با تغییر Baseline اصلی و از طریق ADR مجاز است.**
