using System;

namespace HJ.Server.Domain.Installations;

public class InstallationEnvironment
{
    public Guid Id { get; private set; }
    public Guid InstallationId { get; private set; }
    public string? OSVersion { get; private set; }
    public string? CpuName { get; private set; }
    public int CpuCoreCount { get; private set; }
    public double RamGB { get; private set; }
    public string? ScreenResolution { get; private set; }
    public string? HardwareIdentifier { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private InstallationEnvironment() { }

    private InstallationEnvironment(Guid installationId, string? osVersion, string? cpuName, int cpuCoreCount, double ramGB, string? screenResolution, string? hardwareIdentifier)
    {
        Id = Guid.NewGuid();
        InstallationId = installationId;
        OSVersion = osVersion;
        CpuName = cpuName;
        CpuCoreCount = cpuCoreCount;
        RamGB = ramGB;
        ScreenResolution = screenResolution;
        HardwareIdentifier = hardwareIdentifier;
        CreatedAt = DateTime.UtcNow;
    }

    public static InstallationEnvironment Create(Guid installationId, string? osVersion, string? cpuName, int cpuCoreCount, double ramGB, string? screenResolution, string? hardwareIdentifier)
    {
        if (installationId == Guid.Empty)
            throw new ArgumentException("InstallationId cannot be empty.", nameof(installationId));

        return new InstallationEnvironment(installationId, osVersion, cpuName, cpuCoreCount, ramGB, screenResolution, hardwareIdentifier);
    }
}