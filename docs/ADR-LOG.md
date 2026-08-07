# Architecture Decision Records (ADR)

## ADR-001: Telemetry / Logging Boundary
**Status:** Accepted
**Decision:** Operation, TelemetryEvent, and ApplicationLog are independent Aggregates.

## ADR-002: Tenant Aggregate
**Status:** Accepted
**Decision:** System uses Tenant as a primary Aggregate for multi-tenancy.

## ADR-003: Notification Deferred
**Status:** Accepted
**Decision:** Notification module implementation is deferred to a later phase.

## ADR-004: ProcessingJob Removal
**Status:** Accepted
**Decision:** ProcessingJob concept is completely removed from the Domain.

## ADR-005: BaseEntity Decision
**Status:** Accepted
**Decision:** Domain entities must not rely on external frameworks (like Volo.Abp) and should use the internal BaseEntity.

## ADR-006: Exception Mapping
**Status:** Accepted
**Decision:** Domain exceptions (e.g., InstallationAlreadyExistsException, NotFound) will be mapped to HTTP status codes globally via middleware.