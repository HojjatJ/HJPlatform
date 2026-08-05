$root = "D:\Projects\Visual Studio\HJPlatform"

Write-Host "=== Integration Tests Files ==="

Get-ChildItem `
"$root\tests\HJ.Server.IntegrationTests" `
-Recurse `
-File |
Select-Object FullName


Write-Host ""
Write-Host "=== Unit Tests Files ==="

Get-ChildItem `
"$root\tests\HJ.Server.UnitTests" `
-Recurse `
-File |
Select-Object FullName