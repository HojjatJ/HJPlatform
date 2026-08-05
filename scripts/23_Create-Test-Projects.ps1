$root = "D:\Projects\Visual Studio\HJPlatform"

$testsPath = "$root\tests"


if(!(Test-Path $testsPath))
{
    New-Item -ItemType Directory -Path $testsPath -Force | Out-Null
}


dotnet new xunit `
-n HJ.Server.UnitTests `
-o "$testsPath\HJ.Server.UnitTests"


dotnet new xunit `
-n HJ.Server.IntegrationTests `
-o "$testsPath\HJ.Server.IntegrationTests"


Write-Host "Test projects created."