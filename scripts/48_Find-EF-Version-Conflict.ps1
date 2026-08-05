$root = Split-Path -Parent $PSScriptRoot

Get-ChildItem $root -Recurse -Filter "*.csproj" |
ForEach-Object {
    Select-String -Path $_.FullName `
    -Pattern "EntityFrameworkCore|10.0.4|10.0.10" |
    ForEach-Object {
        "$($_.Path): $($_.Line)"
    }
}