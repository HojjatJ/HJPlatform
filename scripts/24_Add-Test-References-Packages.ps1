$root = "D:\Projects\Visual Studio\HJPlatform"


$unitProject = "$root\tests\HJ.Server.UnitTests\HJ.Server.UnitTests.csproj"

$integrationProject = "$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj"


$applicationProject = "$root\src\HJ.Server.Application\HJ.Server.Application.csproj"

$apiProject = "$root\src\HJ.Server.Api\HJ.Server.Api.csproj"

$infraProject = "$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj"



# References

dotnet add $unitProject reference `
$applicationProject


dotnet add $integrationProject reference `
$apiProject


dotnet add $integrationProject reference `
$infraProject



# Unit packages

dotnet add $unitProject package FluentAssertions

dotnet add $unitProject package Moq



# Integration packages

dotnet add $integrationProject package Microsoft.AspNetCore.Mvc.Testing

dotnet add $integrationProject package FluentAssertions

dotnet add $integrationProject package Testcontainers.PostgreSql



Write-Host "Test references and packages added."