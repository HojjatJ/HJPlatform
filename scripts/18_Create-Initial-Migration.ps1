$root = "D:\Projects\ Visual Studio\HJPlatform".Replace(" \","\")


$apiProject = "$root\src\HJ.Server.Api\HJ.Server.Api.csproj"

$infraProject = "$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj"


dotnet tool install --global dotnet-ef


dotnet add $infraProject package Microsoft.EntityFrameworkCore.Design



dotnet ef migrations add InitialCreate `
--project $infraProject `
--startup-project $apiProject `
--output-dir Persistence/Migrations


Write-Host "Initial migration created."