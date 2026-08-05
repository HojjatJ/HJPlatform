namespace HJ.Server.Domain.Installations;


public class InstallationEnvironment
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public string? OSVersion { get; set; }

    public string? CpuName { get; set; }

    public int CpuCoreCount { get; set; }

    public double RamGB { get; set; }

    public string? ScreenResolution { get; set; }

    public string? HardwareIdentifier { get; set; }

    public DateTime CreatedAt { get; set; }
}
