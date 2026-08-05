$root = "D:\Projects\Visual Studio\HJPlatform"

$file = "$root\src\HJ.Server.Api\Endpoints\Health\HealthEndpoint.cs"


$content = Get-Content $file -Raw


$content = $content.Replace(
@"
await SendAsync(
            new HealthResponse
            {
                Status = "ok",
                Service = "HJPlatform",
                Utc = DateTime.UtcNow
            },
            cancellation: ct);
"@,
@"
await Send.OkAsync(
            new HealthResponse
            {
                Status = "ok",
                Service = "HJPlatform",
                Utc = DateTime.UtcNow
            },
            ct);
"@
)


Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8


Write-Host "Health endpoint fixed."