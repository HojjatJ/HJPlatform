$ErrorActionPreference = "Stop"

Write-Host "Creating Architecture Tests project..."

$projectPath = "tests/HJ.Server.ArchitectureTests"

if (!(Test-Path $projectPath)) {
    dotnet new xunit -n HJ.Server.ArchitectureTests -o $projectPath
}
else {
    Write-Host "Project already exists."
}

Write-Host "Adding project to solution..."

dotnet sln add "$projectPath/HJ.Server.ArchitectureTests.csproj" --in-root

Write-Host "Adding NetArchTest..."

dotnet add "$projectPath/HJ.Server.ArchitectureTests.csproj" package NetArchTest.Rules

Write-Host "Adding project references..."

dotnet add "$projectPath/HJ.Server.ArchitectureTests.csproj" reference `
    "src/HJ.Server.Domain/HJ.Server.Domain.csproj"

dotnet add "$projectPath/HJ.Server.ArchitectureTests.csproj" reference `
    "src/HJ.Server.Application/HJ.Server.Application.csproj"

dotnet add "$projectPath/HJ.Server.ArchitectureTests.csproj" reference `
    "src/HJ.Server.Infrastructure/HJ.Server.Infrastructure.csproj"

dotnet add "$projectPath/HJ.Server.ArchitectureTests.csproj" reference `
    "src/HJ.Server.Api/HJ.Server.Api.csproj"

Write-Host ""
Write-Host "Architecture Tests project created successfully."