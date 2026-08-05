using HJ.Server.Domain.Common;
namespace HJ.Server.Domain.Telemetry;


public class TelemetryEvent : BaseEntity
{
    

    public Guid InstallationId { get; set; }

    public Guid OperationId { get; set; }

    public string EventName { get; set; } = default!;

    public int EventVersion { get; set; }

    public string PayloadJson { get; set; } = default!;

    
}

