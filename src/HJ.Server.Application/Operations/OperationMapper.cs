using HJ.Server.Domain.Operations;
using HJ.Server.Contracts.Operations;

namespace HJ.Server.Application.Operations;

public class OperationMapper
{
    public OperationDto OperationToDto(Operation operation)
    {
        return new OperationDto(
            operation.Id,
            operation.InstallationId,
            operation.CorrelationId,
            operation.Type,
            operation.StartedAt,
            operation.EndedAt,
            operation.Status);
    }
}
