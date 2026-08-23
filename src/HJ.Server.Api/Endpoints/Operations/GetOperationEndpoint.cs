using FastEndpoints;
using HJ.Server.Application.Operations;
using HJ.Server.Contracts.Operations;

namespace HJ.Server.Api.Endpoints.Operations;

public class GetOperationEndpoint : EndpointWithoutRequest<OperationDto>
{
    private readonly IOperationService _service;

    public GetOperationEndpoint(IOperationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/operations/{operationId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var operationId = Route<Guid>("operationId");
        var result = await _service.GetByIdAsync(operationId, ct);
        await SendOkAsync(result, ct);
    }
}
