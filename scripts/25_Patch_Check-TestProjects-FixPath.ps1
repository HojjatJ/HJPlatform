$root = "D:\Projects\Visual Studio\HJPlatform"

Write-Host "=== Unit Test Project ==="
Get-Content "$root\tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj"

Write-Host ""
Write-Host "=== Integration Test Project ==="
Get-Content "$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"