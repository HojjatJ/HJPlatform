using FastEndpoints;
using HJ.Server.Application.Installations;
using HJ.Server.Contracts.Installations;

namespace HJ.Server.Api.Endpoints.Installations;

public class UpdateInstallationVersionEndpoint : Endpoint<UpdateInstallationVersionRequest, InstallationDto>
{
    private readonly IInstallationService _service;

    public UpdateInstallationVersionEndpoint(IInstallationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/api/installations/{installationId}/version");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateInstallationVersionRequest req, CancellationToken ct)
    {
        var installationId = Route<Guid>("installationId");
        var result = await _service.UpdateVersionAsync(installationId, req, ct);
        await SendOkAsync(result, ct);
    }
}
