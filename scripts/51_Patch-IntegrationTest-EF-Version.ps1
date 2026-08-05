$root = Split-Path -Parent $PSScriptRoot

$project = Join-Path $root "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"

$content = Get-Content $project -Raw

if ($content -notmatch 'Microsoft.EntityFrameworkCore"')
{
    $insert = @"

    <PackageReference Include="Microsoft.EntityFrameworkCore" />

    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" />

"@

    $content = $content -replace "(<ItemGroup>)", "`$1$insert"
}

Set-Content $project $content

Write-Host "IntegrationTests EF references patched."