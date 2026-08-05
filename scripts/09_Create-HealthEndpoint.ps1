$root = "D:\Projects\Visual Studio\HJPlatform"

$folder = "$root\src\HJ.Server.Api\Endpoints\Health"


if (!(Test-Path $folder))
{
    New-Item -ItemType Directory -Path $folder | Out-Null
}


$file = "$folder\HealthEndpoint.cs"


$content = @"
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
"@


Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8


Write-Host "Health endpoint created."