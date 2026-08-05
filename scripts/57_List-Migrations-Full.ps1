$root = Split-Path -Parent $PSScriptRoot

$path = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\Migrations"

Write-Host "=== Migration Files ==="

Get-ChildItem $path -File | ForEach-Object {
    $_.FullName
}