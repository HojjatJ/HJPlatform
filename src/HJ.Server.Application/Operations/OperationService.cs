using System;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;
using HJ.Server.Domain.Operations;
using HJ.Server.Domain.Operations.Exceptions;
using HJ.Server.Foundation.Abstractions.Telemetry;
using HJ.Server.Foundation.Abstractions.Logging;

namespace HJ.Server.Application.Operations;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _repository;
    private readonly OperationMapper _mapper;
    private readonly ITelemetryService _telemetryService;
    private readonly ILoggingService _loggingService;

    public OperationService(IOperationRepository repository, OperationMapper mapper, ITelemetryService telemetryService, ILoggingService loggingService)
    {
        _repository = repository;
        _mapper = mapper;
        _telemetryService = telemetryService;
        _loggingService = loggingService;
    }

    public async Task<OperationDto> StartAsync(StartOperationRequest request, CancellationToken cancellationToken)
    {
        var operation = Operation.Create(request.InstallationId, request.Type);
        
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

        operation.Complete(request.Status);

        await _repository.UpdateAsync(operation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        try
        {
            await _telemetryService.TrackEventAsync("OperationCompleted", 1, new { status = request.Status }, operation.InstallationId, operation.Id, cancellationToken);
        }
        catch { }

        return _mapper.OperationToDto(operation);
    }

    public async Task<OperationDto> GetByIdAsync(Guid operationId, CancellationToken cancellationToken)
    {
        try
        {
            var operation = await _repository.GetByIdAsync(operationId, cancellationToken)
                ?? throw new OperationNotFoundException(operationId);

            return _mapper.OperationToDto(operation);
        }
        catch (OperationNotFoundException ex)
        {
            try
            {
                await _loggingService.LogAsync("Error", $"Operation {operationId} not found.", null, operationId, ex, null, cancellationToken);
            }
            catch { }
            throw;
        }
    }
}
