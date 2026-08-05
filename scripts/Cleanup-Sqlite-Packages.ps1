$projects = @(
    "src\HJ.Server.Api\HJ.Server.Api.csproj",
    "src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj",
    "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"
)

$packages = @(
    "SQLitePCLRaw.bundle_e_sqlite3",
    "SQLitePCLRaw.core",
    "SQLitePCLRaw.lib.e_sqlite3",
    "SQLitePCLRaw.provider.e_sqlite3"
)

foreach ($project in $projects) {

    Write-Host "Cleaning $project"

    $path = Resolve-Path $project

    $content = Get-Content $path -Raw

    foreach ($package in $packages) {

        $pattern = '(?s)\s*<PackageReference Include="' + 
                   [regex]::Escape($package) +
                   '".*?</PackageReference>'

        $content = [regex]::Replace(
            $content,
            $pattern,
            ''
        )
    }

    Set-Content $path $content
}

Write-Host "SQLite package cleanup completed."