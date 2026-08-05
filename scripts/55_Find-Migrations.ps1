$root = Split-Path -Parent $PSScriptRoot

Write-Host "=== Searching Migration Files ==="

Get-ChildItem $root -Recurse -Include "*Migration*.cs","*ModelSnapshot.cs" |
    Select-Object FullName