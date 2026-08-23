using System;
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Telemetry;

public class TelemetryEvent : BaseEntity
{
    public Guid InstallationId { get; set; }
    public Guid OperationId { get; set; }
    public string EventName { get; set; } = default!;
    public int EventVersion { get; set; }
    public string PayloadJson { get; set; } = default!;

    public static TelemetryEvent Create(
        string eventName,
        int eventVersion,
        string payloadJson,
        Guid installationId,
        Guid operationId)
    {
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
