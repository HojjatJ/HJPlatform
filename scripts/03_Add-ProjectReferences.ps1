$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


Write-Host "Adding project references..."


dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" reference `
    ".\src\HJ.Server.Application\HJ.Server.Application.csproj" `
    ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" `
    ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj" `
    ".\src\HJ.Server.Contracts\HJ.Server.Contracts.csproj"



dotnet add ".\src\HJ.Server.Application\HJ.Server.Application.csproj" reference `
    ".\src\HJ.Server.Domain\HJ.Server.Domain.csproj" `
    ".\src\HJ.Server.Contracts\HJ.Server.Contracts.csproj" `
    ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj"



dotnet add ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" reference `
    ".\src\HJ.Server.Application\HJ.Server.Application.csproj" `
    ".\src\HJ.Server.Domain\HJ.Server.Domain.csproj" `
    ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj"



dotnet add ".\src\HJ.Server.SDK\HJ.Server.SDK.csproj" reference `
    ".\src\HJ.Server.Contracts\HJ.Server.Contracts.csproj"



dotnet add ".\src\HJ.Server.Contracts\HJ.Server.Contracts.csproj" reference `
    ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj"



Write-Host ""
Write-Host "Project references configured successfully."