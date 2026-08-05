$projects = @(
    ".\src\HJ.Server.Api\HJ.Server.Api.csproj",
    ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj",
    ".\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"
)

$packages = @{
    "SQLitePCLRaw.bundle_e_sqlite3" = "3.0.5"
    "SQLitePCLRaw.core" = "3.0.5"
    "SQLitePCLRaw.provider.e_sqlite3" = "3.0.5"
    "SQLitePCLRaw.lib.e_sqlite3" = "3.50.3"
}

foreach ($project in $projects) {
    foreach ($package in $packages.Keys) {

        Write-Host "Updating $package -> $($packages[$package])"

        dotnet add $project package $package --version $packages[$package]
    }
}

Write-Host "Completed."