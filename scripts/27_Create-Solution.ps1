$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


dotnet new sln `
-n HJPlatform


$projects = @(
    "src\HJ.Server.Api\HJ.Server.Api.csproj",
    "src\HJ.Server.Application\HJ.Server.Application.csproj",
    "src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj",
    "tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj",
    "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"
)


foreach($project in $projects)
{
    dotnet sln HJPlatform.sln add $project
}


Write-Host "Solution created and projects added."