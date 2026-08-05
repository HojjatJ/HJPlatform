$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


Write-Host "Installing API packages..."

dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package FastEndpoints

dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package FastEndpoints.Swagger

dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package Scalar.AspNetCore

dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package Serilog.AspNetCore

dotnet add ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package Hellang.Middleware.ProblemDetails



Write-Host "Installing Application packages..."

dotnet add ".\src\HJ.Server.Application\HJ.Server.Application.csproj" package FluentValidation

dotnet add ".\src\HJ.Server.Application\HJ.Server.Application.csproj" package Riok.Mapperly



Write-Host "Installing Infrastructure packages..."

dotnet add ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" package Microsoft.EntityFrameworkCore

dotnet add ".\src\HJ.Server.Infrastructure\HJ.Server.Infrastructure.csproj" package Npgsql.EntityFrameworkCore.PostgreSQL



Write-Host "Installing Foundation packages..."

dotnet add ".\src\HJ.Server.Foundation\HJ.Server.Foundation.csproj" package Ardalis.Result



Write-Host ""

Write-Host "NuGet packages installed successfully."