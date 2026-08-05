$root = "D:\Projects\Visual Studio\HJPlatform"

$apiProject = "$root\src\HJ.Server.Api\HJ.Server.Api.csproj"

$infraProject = "$root\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj"


dotnet ef migrations add InitialCreate `
--context HJDbContext `
--project $infraProject `
--startup-project $apiProject `
--output-dir Persistence/Migrations


Write-Host "Migration created for HJDbContext."