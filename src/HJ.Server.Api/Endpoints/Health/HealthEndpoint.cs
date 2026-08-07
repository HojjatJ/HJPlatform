using FastEndpoints;

namespace HJ.Server.Api.Endpoints.Health;


public class HealthEndpoint : EndpointWithoutRequest<HealthResponse>
{
    public override void Configure()
    {
        Get("/api/health");

        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Returns API health status.";
            s.Description = "Used for monitoring and availability checks.";
        });
    }


    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(
            new HealthResponse
            {
                Status = "ok",
                Service = "HJPlatform",
                Utc = DateTime.UtcNow
            },
            cancellation: ct);
    }
}


public class HealthResponse
{
    public string Status { get; set; } = default!;

    public string Service { get; set; } = default!;

    public DateTime Utc { get; set; }
}