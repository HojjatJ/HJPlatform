$root = "D:\Projects\Visual Studio\HJPlatform"


$folders = @(

"$root\src\HJ.Server.Domain\Installations",
"$root\src\HJ.Server.Domain\Operations",
"$root\src\HJ.Server.Domain\Telemetry",
"$root\src\HJ.Server.Domain\Logging",
"$root\src\HJ.Server.Domain\Optimization"

)


foreach($folder in $folders)
{
    if(!(Test-Path $folder))
    {
        New-Item -ItemType Directory -Path $folder | Out-Null
    }
}



@"
namespace HJ.Server.Domain.Installations;


public class Installation
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public string AppId { get; set; } = default!;

    public string CurrentVersion { get; set; } = default!;

    public DateTime FirstSeenAt { get; set; }

    public DateTime LastSeenAt { get; set; }
}
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Installations\Installation.cs" `
-Encoding UTF8



@"
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
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Installations\InstallationEnvironment.cs" `
-Encoding UTF8



@"
namespace HJ.Server.Domain.Operations;


public class Operation
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public Guid CorrelationId { get; set; }

    public string Type { get; set; } = default!;

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string Status { get; set; } = default!;
}
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Operations\Operation.cs" `
-Encoding UTF8



@"
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
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Telemetry\TelemetryEvent.cs" `
-Encoding UTF8



@"
namespace HJ.Server.Domain.Logging;


public class ApplicationLog
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public Guid OperationId { get; set; }

    public string Level { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string? ExceptionJson { get; set; }

    public string? PropertiesJson { get; set; }

    public DateTime CreatedAt { get; set; }
}
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Logging\ApplicationLog.cs" `
-Encoding UTF8



@"
namespace HJ.Server.Domain.Optimization;


public class OptimizationBatch
{
    public Guid Id { get; set; }

    public Guid OperationId { get; set; }

    public string BatchId { get; set; } = default!;

    public string ExecutionSource { get; set; } = default!;

    public int FilesCount { get; set; }

    public int SuccessCount { get; set; }

    public int FailedCount { get; set; }

    public long TargetSizeKB { get; set; }

    public long SavedBytes { get; set; }

    public long DurationMs { get; set; }

    public string? ProcessingMode { get; set; }

    public int ConcurrencyLevel { get; set; }

    public DateTime CreatedAt { get; set; }
}
"@ | Set-Content `
"$root\src\HJ.Server.Domain\Optimization\OptimizationBatch.cs" `
-Encoding UTF8



Write-Host "Domain entities created successfully."