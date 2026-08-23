using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;

namespace HJ.Server.Application.Operations;

public interface IOperationService
{
    Task<OperationDto> StartAsync(StartOperationRequest request, CancellationToken cancellationToken);
    Task<OperationDto> CompleteAsync(Guid operationId, CompleteOperationRequest request, CancellationToken cancellationToken);
    Task<OperationDto> GetByIdAsync(Guid operationId, CancellationToken cancellationToken);
}
