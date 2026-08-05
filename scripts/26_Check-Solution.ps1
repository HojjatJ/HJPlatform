$root = "D:\Projects\Visual Studio\HJPlatform"

Write-Host "=== Solution Files ==="

Get-ChildItem $root -Filter "*.sln" |
Select-Object FullName


Write-Host ""
Write-Host "=== Projects in Solution ==="

$sln = Get-ChildItem $root -Filter "*.sln" | Select-Object -First 1

if ($null -ne $sln)
{
    dotnet sln $sln.FullName list
}
else
{
    Write-Host "No solution found."
}