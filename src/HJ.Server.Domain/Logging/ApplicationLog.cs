using System;
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Logging;

public class ApplicationLog : BaseEntity
{
    public Guid InstallationId { get; private set; }
    public Guid OperationId { get; private set; }
    public string Level { get; private set; } = default!;
    public string Message { get; private set; } = default!;
    public string? ExceptionJson { get; private set; }
    public string? PropertiesJson { get; private set; }

    private ApplicationLog()
    {
    }

    public static ApplicationLog Create(
        string level,
        string message,
        Guid installationId,
        Guid operationId,
        string? exceptionJson,
        string? propertiesJson)
    {
        if (string.IsNullOrWhiteSpace(level))
            throw new ArgumentException("Level cannot be null or whitespace.", nameof(level));
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));
        if (installationId == Guid.Empty)
            throw new ArgumentException("Installation ID cannot be empty.", nameof(installationId));
        if (operationId == Guid.Empty)
            throw new ArgumentException("Operation ID cannot be empty.", nameof(operationId));

        return new ApplicationLog
        {
            Id = Guid.NewGuid(),
            Level = level,
            Message = message,
            InstallationId = installationId,
            OperationId = operationId,
            ExceptionJson = exceptionJson,
            PropertiesJson = propertiesJson,
            CreatedAt = DateTime.UtcNow
        };
    }
}
