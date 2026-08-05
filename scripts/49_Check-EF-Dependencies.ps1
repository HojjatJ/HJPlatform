$root = Split-Path -Parent $PSScriptRoot

dotnet list "$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" package --include-transitive |
Select-String "EntityFrameworkCore|Npgsql"

Write-Host ""
Write-Host "=== API ==="

dotnet list "$root\src\HJ.Server.Api\HJ.Server.Api.csproj" package --include-transitive |
Select-String "EntityFrameworkCore|Npgsql"