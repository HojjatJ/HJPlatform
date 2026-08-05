$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root

Write-Host "Creating solution..."

dotnet new sln -n HJPlatform


Write-Host "Creating projects..."


dotnet new web -n HJ.Server.Api `
    -o ".\src\HJ.Server.Api"


dotnet new classlib -n HJ.Server.Application `
    -o ".\src\HJ.Server.Application"


dotnet new classlib -n HJ.Server.Domain `
    -o ".\src\HJ.Server.Domain"


dotnet new classlib -n HJ.Server.Infrastructure `
    -o ".\src\HJ.Server.Infrastructure"


dotnet new classlib -n HJ.Server.Contracts `
    -o ".\src\HJ.Server.Contracts"


dotnet new classlib -n HJ.Server.Foundation `
    -o ".\src\HJ.Server.Foundation"


dotnet new classlib -n HJ.Server.SDK `
    -o ".\src\HJ.Server.SDK"



Write-Host "Adding projects to solution..."


dotnet sln add `
    ".\src\HJ.Server.Api\HJ.Server.Api.csproj" `
    ".\src\HJ.Server.Application\HJ.Server.Application.csproj" `
    ".\src\HJ.Server.Domain\HJ.Server.Domain.csproj" `
    ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" `
    ".\src\HJ.Server.Contracts\HJ.Server.Contracts.csproj" `
    ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj" `
    ".\src\HJ.Server.SDK\HJ.Server.SDK.csproj"


Write-Host ""
Write-Host "HJPlatform projects created successfully."