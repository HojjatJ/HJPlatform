using System;

namespace HJ.Server.Domain.Operations.Exceptions;

public class OperationNotFoundException : Exception
{
    public Guid OperationId { get; }

    public OperationNotFoundException(Guid operationId) 
        : base($"Operation with ID '{operationId}' was not found.")
    {
        OperationId = operationId;
    }
}
