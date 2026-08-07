using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using HJ.Server.Contracts.Installations;
using HJ.Server.Application.Installations;

namespace HJ.Server.Api.Endpoints.Installations;

public class GetInstallationEndpoint : EndpointWithoutRequest<InstallationDto>
{
    private readonly IInstallationService _service;

    public GetInstallationEndpoint(IInstallationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/installations/{installationId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var installationId = Route<Guid>("installationId");
        var result = await _service.GetAsync(installationId, ct);
        
        await SendAsync(result, cancellation: ct);
    }
}