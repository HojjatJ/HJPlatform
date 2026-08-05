$csproj = ".\src\HJ.Server.Application\HJ.Server.Application.csproj"

dotnet add $csproj package Microsoft.Extensions.DependencyInjection.Abstractions

Write-Host "Package added."