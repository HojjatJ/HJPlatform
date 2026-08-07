using System;

namespace HJ.Server.Contracts.Installations;

public class RecordHeartbeatRequest
{
    public Guid? ProductVersionId { get; set; }
    public string? OSVersion { get; set; }
    public string? CpuName { get; set; }
    public int? CpuCoreCount { get; set; }
    public double? RamGB { get; set; }
    public string? ScreenResolution { get; set; }
    public string? HardwareIdentifier { get; set; }
}