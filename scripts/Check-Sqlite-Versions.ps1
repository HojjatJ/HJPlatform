dotnet list package --outdated --include-transitive | Select-String "SQLitePCLRaw"
$ErrorActionPreference = "Stop"

Write-Host "Building solution..."

dotnet build

Write-Host ""
Write-Host "Checking assemblies..."

Get-ChildItem -Path .\src -Filter *.dll -Recurse |
Where-Object {
    $_.FullName -like "*bin\Debug\net10.0\*"
} |
Select-Object FullName