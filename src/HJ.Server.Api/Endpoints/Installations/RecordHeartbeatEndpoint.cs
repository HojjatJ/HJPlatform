using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using HJ.Server.Contracts.Installations;
using HJ.Server.Application.Installations;

namespace HJ.Server.Api.Endpoints.Installations;

public class RecordHeartbeatEndpoint : Endpoint<RecordHeartbeatRequest>
{
    private readonly IInstallationService _service;

    public RecordHeartbeatEndpoint(IInstallationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/installations/{installationId}/heartbeats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RecordHeartbeatRequest req, CancellationToken ct)
    {
        var installationId = Route<Guid>("installationId");
        
        await _service.RecordHeartbeatAsync(installationId, req, ct);
        
        await SendOkAsync(cancellation: ct);
    }
}