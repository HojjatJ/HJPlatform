# ADR-002: Entity Base Standard

## Status
Accepted

## Context

برخی Entityها از BaseEntity استفاده می‌کردند و برخی Entityها دارای پیاده‌سازی مستقل برای Identity و Audit بودند.

این موضوع باعث تفاوت در رفتار Entityها می‌شد.

## Decision

تمام Entityهای دامنه که نیاز به Identity و Audit دارند باید از BaseEntity ارث‌بری کنند.

BaseEntity مسئول موارد مشترک زیر است:

- Id
- TenantId
- CreatedAt
- ModifiedAt

منطق کسب‌وکار باید داخل خود Entity باقی بماند.

## Consequences

مزایا:

- استاندارد شدن مدل دامنه
- آماده شدن برای قابلیت‌های مشترک آینده
- کاهش تکرار کد

محدودیت:

- Entityهای خاص که رفتار متفاوت دارند باید با تصمیم معماری جداگانه بررسی شوند.

## Scope

این استاندارد برای Aggregate Rootها و Entityهای دامنه اعمال می‌شود.