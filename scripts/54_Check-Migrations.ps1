$root = Split-Path -Parent $PSScriptRoot

$migrations = Join-Path $root "src\HJ.Server.Infrastructure\Migrations"

Write-Host "=== EF Migrations ==="

if (Test-Path $migrations)
{
    Get-ChildItem $migrations -File | Select-Object Name
}
else
{
    Write-Host "No migrations folder found."
}