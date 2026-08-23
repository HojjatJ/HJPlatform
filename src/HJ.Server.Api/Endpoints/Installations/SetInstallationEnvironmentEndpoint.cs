using FastEndpoints;
using HJ.Server.Application.Installations;
using HJ.Server.Contracts.Installations;

namespace HJ.Server.Api.Endpoints.Installations;

public class SetInstallationEnvironmentEndpoint : Endpoint<SetInstallationEnvironmentRequest, InstallationDto>
{
    private readonly IInstallationService _service;

    public SetInstallationEnvironmentEndpoint(IInstallationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/api/installations/{installationId}/environment");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SetInstallationEnvironmentRequest req, CancellationToken ct)
    {
        var installationId = Route<Guid>("installationId");
        var result = await _service.SetEnvironmentAsync(installationId, req, ct);
        await SendOkAsync(result, ct);
    }
}
