$root = Split-Path -Parent $PSScriptRoot

$path = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\Configurations"

Write-Host "=== Product EF Configurations ==="

Get-ChildItem $path -Filter "*Product*.cs" |
Select-Object FullName

Write-Host ""

Get-ChildItem $path -Filter "*Tenant*.cs" |
Select-Object FullName