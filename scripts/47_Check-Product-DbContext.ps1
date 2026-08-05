$root = Split-Path -Parent $PSScriptRoot

$dbContextPath = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\HJDbContext.cs"

Write-Host "=== HJDbContext Product Check ==="

Select-String -Path $dbContextPath -Pattern `
"Product|Tenant"