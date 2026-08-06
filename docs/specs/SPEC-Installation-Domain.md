# Spec — Installation Management — Layer: Domain

**Pipeline step:** 1 و 2 از ۱۱ (Domain + Repository Interface) — طبق `ARCHITECTURE-RULES.md` بخش ۱۲
**Aggregate:** `Installation` (Root) + `InstallationEnvironment` (Entity داخلی، غیر-Root)
**پروژه هدف:** `HJ.Server.Domain`
**Namespace:** `HJ.Server.Domain.Installations`

> این Spec فقط لایه‌ی Domain را پوشش می‌دهد. **در این مرحله چیزی خارج از `HJ.Server.Domain` تولید نکن** — بدون EF Configuration، بدون Migration، بدون Repository Implementation، بدون Application Service، بدون DTO، بدون Endpoint. اگر نیاز به این‌ها احساس کردی، متوقف شو و بگو، خودسرانه اضافه نکن.
> مرجع طراحی: `DOMAIN-MODEL.md` (بخش Installation و InstallationEnvironment). مرجع قوانین: `ARCHITECTURE-RULES.md`.

---

## 1. Entity: `Installation` (Aggregate Root)

### Base
از `BaseEntity` (پروژه `HJ.Server.Foundation`) ارث‌بری می‌کند → `Id`, `CreatedAt`, `ModifiedAt`, `TenantId` از آنجا می‌آید. دوباره تعریف نکن.

### Properties (فقط با `private set` — تغییر فقط از طریق متد)

| Property | Type | توضیح |
|---|---|---|
| `InstallationId` | `Guid` | شناسه‌ی دائمی نصب که کلاینت با خودش نگه می‌دارد. **این با `Id` (PK از BaseEntity) فرق دارد.** یکتا در کل سیستم. |
| `ProductId` | `Guid` | محصولی که نصب شده. غیرقابل‌تغییر بعد از ساخت. |
| `ProductVersionId` | `Guid` | نسخه‌ی فعلاً نصب‌شده. تنها یک نسخه‌ی فعال در هر لحظه (طبق قانون دامنه). |
| `FirstSeenAt` | `DateTime` (UTC) | زمان اولین ثبت. |
| `LastSeenAt` | `DateTime` (UTC) | آخرین باری که این Installation دیده شده (heartbeat/آخرین فعالیت). |
| `Environment` | `InstallationEnvironment?` | Navigation property، صفر یا یک. |

### متدهای رفتاری (نه Setter مستقیم)

```csharp
// Factory — تنها راه ساخت یک Installation جدید
public static Installation Create(Guid installationId, Guid productId, Guid productVersionId, Guid? tenantId)

// آپدیت زمان آخرین دیده‌شدن — وقتی کلاینت heartbeat/فعالیت می‌فرستد
public void RecordHeartbeat()

// تغییر نسخه‌ی نصب‌شده — وقتی کلاینت آپدیت می‌کند
public void UpdateVersion(Guid newProductVersionId)

// اتصال یا جایگزینی اطلاعات محیط سخت‌افزاری
public void SetEnvironment(InstallationEnvironment environment)
```

### قوانین کسب‌وکار (Domain Validation)

- `installationId`, `productId`, `productVersionId` نباید `Guid.Empty` باشند → در `Create` چک شود، در غیر این صورت `ArgumentException` استاندارد (این سطح validation ورودی است، نه Business Rule سطح بالا).
- `ProductId` بعد از ساخت **غیرقابل‌تغییر** است — هیچ متدی برای تغییرش وجود ندارد.
- یکتایی `InstallationId` در سطح Domain enforce نمی‌شود (Entity از دیتابیس خبر ندارد) — این در Application layer (چک از طریق Repository) و در Database (Unique Index) enforce می‌شود. **این خودش یک تصمیم است، نه فراموشی — مثل الگوی Product.**
- `UpdateVersion` باید `LastSeenAt` را هم آپدیت کند (چون تغییر نسخه یعنی کلاینت الان فعال بوده).
- `SetEnvironment` جایگزینی کامل انجام می‌دهد (یک Installation فقط یک Environment دارد؛ Environment تغییرات جزئی/partial ندارد — کل رکورد جایگزین می‌شود، ساده‌تر از update تکه‌تکه).

---

## 2. Entity: `InstallationEnvironment` (غیر-Root، داخل Aggregate)

### Properties

| Property | Type |
|---|---|
| `Id` | `Guid` |
| `InstallationId` | `Guid` (FK به `Installation.Id`، نه `Installation.InstallationId`) |
| `OSVersion` | `string?` |
| `CpuName` | `string?` |
| `CpuCoreCount` | `int` |
| `RamGB` | `double` |
| `ScreenResolution` | `string?` |
| `HardwareIdentifier` | `string?` |
| `CreatedAt` | `DateTime` (UTC) |

### قوانین

- **Repository جدا برای این Entity نساز.** فقط از طریق `Installation` (Aggregate Root) در دسترس است — این دقیقاً همون چیزیه که در `ARCHITECTURE-RULES.md` بخش ۴.۲ گفته شده.
- Factory: `public static InstallationEnvironment Create(Guid installationId, string? osVersion, string? cpuName, int cpuCoreCount, double ramGB, string? screenResolution, string? hardwareIdentifier)`
- `HardwareIdentifier` عمداً `string?` و بدون قانون خاص است — طبق `domain-model.md`، تصمیم پیاده‌سازی دقیقش هنوز باز است. **فرمت یا الگوریتم خاصی براش حدس نزن.**

---

## 3. Domain Exceptions

مکان: `HJ.Server.Domain.Installations.Exceptions` (هم‌الگو با نمونه‌ی موجود Product، مثل `ProductAlreadyExistsException`)

```csharp
public class InstallationAlreadyExistsException : Exception
// وقتی در Application layer تلاش شود یک InstallationId تکراری ثبت شود

public class InstallationNotFoundException : Exception
// برای استفاده‌ی آینده در Application layer (GetById و مشابه)
```

هر دو باید سازنده‌ای داشته باشند که پیام مناسب با شناسه‌ی مربوطه بسازد (مثل الگوی موجود در Product).

---

## 4. Repository Interface

مکان: `HJ.Server.Domain.Installations.IInstallationRepository`

```csharp
public interface IInstallationRepository
{
    Task<Installation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Installation?> GetByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken);
    Task<bool> ExistsByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken);
    Task AddAsync(Installation installation, CancellationToken cancellationToken);
}
```

- بدون متد `Update` صریح — تغییرات از طریق EF Change Tracking در Infrastructure انجام می‌شود (این تصمیم را تغییر نده، فقط این مرحله را طبق همین Interface پیاده کن؛ اگر فکر می‌کنی نیاز به متد دیگری هست، بگو، اضافه نکن).
- Implementation این Interface بخشی از این Spec **نیست** — مرحله‌ی بعدی (Infrastructure) است.

---

## 5. پیشنهاد من درباره‌ی تست (نیاز به تایید شما)

طبق پایپ‌لاین رسمی، "Tests" مرحله‌ی ۹ است (بعد از API/SDK). ولی چون Unit Test منطق Domain به هیچ لایه‌ی دیگری وابسته نیست، پیشنهاد می‌کنم Unit Test های زیر را **همین حالا** هم بنویسیم (استثنای منطقی، نه دور زدن قانون):

- `Installation.Create` با ورودی معتبر → Entity درست ساخته می‌شود
- `Installation.Create` با `Guid.Empty` → Exception
- `RecordHeartbeat` → `LastSeenAt` آپدیت می‌شود
- `UpdateVersion` → `ProductVersionId` و `LastSeenAt` هر دو آپدیت می‌شوند
- `SetEnvironment` → `Environment` درست ست می‌شود

اگه با این استثنا موافقید بگید، وگرنه تست‌ها رو کامل به مرحله‌ی ۹ موکول می‌کنیم.

---

## 6. فرمت خروجی (اجباری)

طبق `ARCHITECTURE-RULES.md` بخش ۱۵، خروجی را **فقط** به فرمت Manifest زیر بده — نه بلوک کد جدا، نه اسکریپت PowerShell:

```
===FILE: src/HJ.Server.Domain/Installations/Installation.cs
<کد کامل>
===ENDFILE===
===FILE: src/HJ.Server.Domain/Installations/InstallationEnvironment.cs
<کد کامل>
===ENDFILE===
===FILE: src/HJ.Server.Domain/Installations/IInstallationRepository.cs
<کد کامل>
===ENDFILE===
===FILE: src/HJ.Server.Domain/Installations/Exceptions/InstallationAlreadyExistsException.cs
<کد کامل>
===ENDFILE===
===FILE: src/HJ.Server.Domain/Installations/Exceptions/InstallationNotFoundException.cs
<کد کامل>
===ENDFILE===
```

(اگر بخش ۵ همین Spec — یعنی نوشتن Unit Test همین حالا — تایید شد، فایل‌های تست هم با همین فرمت، در `tests/HJ.Server.UnitTests/Installations/` اضافه شوند.)

مسیرهای بالا پیش‌فرض بر اساس ساختار پروژه‌ی موجود هستند؛ اگر پوشه‌ی `Installations` با نام دیگری در پروژه مرسوم است، همان الگو را برای Product (`src/HJ.Server.Domain/Products/`) پیگیری کن.

### نحوه‌ی اعمال
```powershell
./tools/apply-files.ps1 -ManifestPath .\manifest.txt
```

### بعد از اعمال، این‌ها را اجرا و نتیجه را برای Review بفرست
```powershell
dotnet build
dotnet test
```

---

## 7. Definition of Done این مرحله

- [ ] پروژه `HJ.Server.Domain` بدون هیچ `using` از EF Core / ASP.NET Core / FastEndpoints build می‌شود.
- [ ] هیچ Property با `public set` وجود ندارد.
- [ ] `Installation.ProductId` بعد از ساخت قابل تغییر نیست.
- [ ] `InstallationEnvironment` بدون Repository مستقل است.
- [ ] Architecture Tests موجود (اگر قانون Dependency را چک می‌کنند) باید پاس شوند.

کد را که آماده کردی برام بفرست تا Review کنم.
