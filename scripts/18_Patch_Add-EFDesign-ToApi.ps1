$root = "D:\Projects\Visual Studio\HJPlatform"

$apiProject = "$root\src\HJ.Server.Api\HJ.Server.Api.csproj"


dotnet add $apiProject package Microsoft.EntityFrameworkCore.Design


Write-Host "EF Design package added to API."