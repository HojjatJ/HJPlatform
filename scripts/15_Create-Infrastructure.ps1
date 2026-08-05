$root = "D:\Projects\Visual Studio\HJPlatform"


$infraPath = "$root\src\HJ.Server.Infrastructure"


if(!(Test-Path $infraPath))
{
    dotnet new classlib `
        -n HJ.Server.Infrastructure `
        -o $infraPath `
        --framework net10.0
}


dotnet add "$infraPath\HJ.Server.Infrastructure.csproj" package Microsoft.EntityFrameworkCore

dotnet add "$infraPath\HJ.Server.Infrastructure.csproj" package Microsoft.EntityFrameworkCore.Design

dotnet add "$infraPath\HJ.Server.Infrastructure.csproj" package Npgsql.EntityFrameworkCore.PostgreSQL



dotnet add "$infraPath\HJ.Server.Infrastructure.csproj" reference `
"$root\src\HJ.Server.Domain\HJ.Server.Domain.csproj"



Write-Host "Infrastructure project created."