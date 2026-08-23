using System;

namespace HJ.Server.Contracts.Operations;

public record OperationDto(
    Guid Id,
    Guid InstallationId,
    Guid CorrelationId,
    string Type,
    DateTime StartedAt,
    DateTime? EndedAt,
    string Status);
