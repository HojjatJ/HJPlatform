$ErrorActionPreference = "Stop"

Write-Host "Checking vulnerable packages..." -ForegroundColor Cyan

dotnet list package --vulnerable --include-transitive

Write-Host ""
Write-Host "Checking SQLite dependency tree..." -ForegroundColor Cyan

dotnet list package --include-transitive |
    Select-String "SQLite|sqlite|EFCore|EntityFramework"