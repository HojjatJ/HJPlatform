using System;
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Telemetry;

public class TelemetryEvent : BaseEntity
{
    public Guid InstallationId { get; private set; }
    public Guid OperationId { get; private set; }
    public string EventName { get; private set; } = default!;
    public int EventVersion { get; private set; }
    public string PayloadJson { get; private set; } = default!;

    private TelemetryEvent()
    {
    }

    public static TelemetryEvent Create(
        string eventName,
        int eventVersion,
        string payloadJson,
        Guid installationId,
        Guid operationId)
    {
        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentException("Event name cannot be null or whitespace.", nameof(eventName));
        if (eventVersion <= 0)
            throw new ArgumentOutOfRangeException(nameof(eventVersion), "Event version must be greater than 0.");
        if (string.IsNullOrWhiteSpace(payloadJson))
            throw new ArgumentException("Payload JSON cannot be null or empty.", nameof(payloadJson));
        if (installationId == Guid.Empty)
            throw new ArgumentException("Installation ID cannot be empty.", nameof(installationId));
        if (operationId == Guid.Empty)
            throw new ArgumentException("Operation ID cannot be empty.", nameof(operationId));

        return new TelemetryEvent
        {
            Id = Guid.NewGuid(),
            EventName = eventName,
            EventVersion = eventVersion,
            PayloadJson = payloadJson,
            InstallationId = installationId,
            OperationId = operationId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
