$root = Split-Path -Parent $PSScriptRoot

$file = Join-Path $root "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"

Write-Host "=== EF References ==="

Select-String -Path $file -Pattern "EntityFrameworkCore"