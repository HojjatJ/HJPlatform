$root = Split-Path -Parent $PSScriptRoot

$file = Join-Path $root "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"

$content = Get-Content $file -Raw

$lines = $content -split "`r?`n"

$seenEF = @{}

$result = foreach ($line in $lines)
{
    if ($line -match '<PackageReference Include="Microsoft.EntityFrameworkCore"')
    {
        if ($seenEF["EF"]) { continue }
        $seenEF["EF"] = $true
    }

    if ($line -match '<PackageReference Include="Microsoft.EntityFrameworkCore.Relational"')
    {
        if ($seenEF["Relational"]) { continue }
        $seenEF["Relational"] = $true
    }

    $line
}

Set-Content $file ($result -join "`r`n")

Write-Host "Duplicate EF PackageReferences removed."