using System;
using HJ.Server.Foundation.Exceptions;

namespace HJ.Server.Domain.Operations.Exceptions;

public class OperationAlreadyCompletedException : HJException
{
    public OperationAlreadyCompletedException(Guid operationId) 
        : base("OPERATION_ALREADY_COMPLETED", $"Operation {operationId} has already been completed.")
    {
    }
}
