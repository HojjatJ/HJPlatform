$root = "D:\Projects\Visual Studio\HJPlatform"

$solution = Get-ChildItem $root -Filter "*.sln" | Select-Object -First 1


if ($null -eq $solution)
{
    Write-Host "Solution file not found."
    exit
}


dotnet sln $solution.FullName add `
"$root\tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj"


dotnet sln $solution.FullName add `
"$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"


Write-Host "Test projects added to solution."