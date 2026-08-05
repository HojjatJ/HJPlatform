$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


$solution = "HJPlatform.slnx"


$projects = @(
    "src\HJ.Server.Api\HJ.Server.Api.csproj",
    "src\HJ.Server.Application\HJ.Server.Application.csproj",
    "src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj",
    "tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj",
    "tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"
)


foreach($project in $projects)
{
    dotnet sln $solution add $project
}


Write-Host "Projects added to HJPlatform.slnx."