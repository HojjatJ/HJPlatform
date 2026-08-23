using FastEndpoints;
using HJ.Server.Application.Operations;
using HJ.Server.Contracts.Operations;
using HJ.Server.Contracts.Operations.Requests;

namespace HJ.Server.Api.Endpoints.Operations;

public class StartOperationEndpoint : Endpoint<StartOperationRequest, OperationDto>
{
    private readonly IOperationService _service;

    public StartOperationEndpoint(IOperationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/operations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(StartOperationRequest req, CancellationToken ct)
    {
        var result = await _service.StartAsync(req, ct);
        await SendCreatedAtAsync<StartOperationEndpoint>("/api/operations/" + result.Id, result, cancellation: ct);
    }
}
