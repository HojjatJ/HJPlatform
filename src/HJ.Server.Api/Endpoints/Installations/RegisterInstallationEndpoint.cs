using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using HJ.Server.Contracts.Installations;
using HJ.Server.Application.Installations;

namespace HJ.Server.Api.Endpoints.Installations;

public class RegisterInstallationEndpoint : Endpoint<RegisterInstallationRequest, InstallationDto>
{
    private readonly IInstallationService _service;

    public RegisterInstallationEndpoint(IInstallationService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/installations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterInstallationRequest req, CancellationToken ct)
    {
        var result = await _service.RegisterAsync(req, ct);
        
        await SendCreatedAtAsync<GetInstallationEndpoint>(
            new { installationId = result.InstallationId }, result, generateAbsoluteUrl: true, cancellation: ct);
    }
}