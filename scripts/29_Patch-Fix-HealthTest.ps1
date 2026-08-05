$root = "D:\Projects\Visual Studio\HJPlatform"

$file = "$root\tests\HJ.Server.IntegrationTests\HealthEndpointTests.cs"


$content = Get-Content $file -Raw


$content = $content.Replace(
    '"/health"',
    '"/api/health"'
)


Set-Content $file $content


Write-Host "Health test path fixed."