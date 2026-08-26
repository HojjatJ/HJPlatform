using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;
using HJ.Server.Domain.Operations;
using HJ.Server.Domain.Operations.Exceptions;
using HJ.Server.Foundation.Abstractions.Telemetry;

namespace HJ.Server.Application.Operations;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _repository;
    private readonly OperationMapper _mapper;
    private readonly ITelemetryService _telemetryService;

    public OperationService(IOperationRepository repository, OperationMapper mapper, ITelemetryService telemetryService)
    {
        _repository = repository;
        _mapper = mapper;
        _telemetryService = telemetryService;
    }

    public async Task<OperationDto> StartAsync(StartOperationRequest request, CancellationToken cancellationToken)
    {
        var operation = Operation.Create(request.InstallationId, request.Type, request.TenantId);

        await _repository.AddAsync(operation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        try
        {
            await _telemetryService.TrackEventAsync("OperationStarted", 1, new { type = request.Type }, request.InstallationId, operation.Id, cancellationToken);
        }
        catch { }

        return _mapper.OperationToDto(operation);
    }

    public async Task<OperationDto> CompleteAsync(Guid operationId, CompleteOperationRequest request, CancellationToken cancellationToken)
    {
        var operation = await _repository.GetByIdAsync(operationId, cancellationToken)        
            ?? throw new OperationNotFoundException(operationId);

        if (operation.Status == OperationStatus.Completed)
            throw new OperationAlreadyCompletedException(operationId);

        operation.Complete(MapStatusFromDto(request.Status));

        await _repository.UpdateAsync(operation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        try
        {
            await _telemetryService.TrackEventAsync("OperationCompleted", 1, new { status = request.Status.ToString() }, operation.InstallationId, operation.Id, cancellationToken);
        }
        catch { }

        return _mapper.OperationToDto(operation);
    }

    private OperationStatus MapStatusFromDto(OperationStatusDto status) => status switch
    {
        OperationStatusDto.Started => OperationStatus.Started,
        OperationStatusDto.Completed => OperationStatus.Completed,
        OperationStatusDto.Failed => OperationStatus.Failed,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    public async Task<OperationDto> GetByIdAsync(Guid operationId, CancellationToken cancellationToken)
    {
        var operation = await _repository.GetByIdAsync(operationId, cancellationToken)    
            ?? throw new OperationNotFoundException(operationId);

        return _mapper.OperationToDto(operation);
    }
}

