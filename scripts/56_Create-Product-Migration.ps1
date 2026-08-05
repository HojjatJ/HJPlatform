$root = Split-Path -Parent $PSScriptRoot

dotnet ef migrations add InitialProductManagement `
    --project "$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" `
    --startup-project "$root\src\HJ.Server.Api\HJ.Server.Api.csproj" `
    --context HJDbContext `
    --output-dir Persistence\Migrations

Write-Host "Product migration created."