namespace HJ.Server.Contracts.Installations;

public record InstallationEnvironmentRequest(
    string? OSVersion,
    string? CpuName,
    int CpuCoreCount,
    double RamGB,
    string? ScreenResolution,
    string? HardwareIdentifier);
