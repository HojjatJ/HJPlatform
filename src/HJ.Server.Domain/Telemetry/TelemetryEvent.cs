namespace HJ.Server.Domain.Telemetry;


public class TelemetryEvent
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public Guid OperationId { get; set; }

    public string EventName { get; set; } = default!;

    public int EventVersion { get; set; }

    public string PayloadJson { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}
