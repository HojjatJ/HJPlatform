using FastEndpoints;
using HJ.Server.Application.Operations;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;

namespace HJ.Server.Api.Endpoints.Operations;

public class CompleteOperationEndpoint : Endpoint<CompleteOperationRequest, OperationDto>
{
    private readonly IOperationService _service;

    public CompleteOperationEndpoint(IOperationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/api/operations/{operationId}/complete");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CompleteOperationRequest req, CancellationToken ct)
    {
        var operationId = Route<Guid>("operationId");
        var result = await _service.CompleteAsync(operationId, req, ct);
        await SendOkAsync(result, ct);
    }
}
