$ErrorActionPreference = "Stop"

Write-Host "Checking SQLite dependency tree..." -ForegroundColor Cyan

$projects = Get-ChildItem -Path . -Recurse -Filter *.csproj

foreach ($project in $projects) {
    Write-Host ""
    Write-Host "Project: $($project.FullName)" -ForegroundColor Yellow

    dotnet list $project.FullName package --include-transitive |
        Select-String "SQLite|sqlite|EntityFramework|EFCore"
}