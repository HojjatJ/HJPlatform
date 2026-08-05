$root = "D:\Projects\Visual Studio\HJPlatform"

$unit = "$root\tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj"
$integration = "$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"


dotnet add $unit package Microsoft.NET.Test.Sdk

dotnet add $integration package Microsoft.NET.Test.Sdk

dotnet add $unit package xunit.runner.visualstudio

dotnet add $integration package xunit.runner.visualstudio


Write-Host "Test discovery packages added."