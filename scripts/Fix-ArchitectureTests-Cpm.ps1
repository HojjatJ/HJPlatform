$ErrorActionPreference = "Stop"

$project = "tests/HJ.Server.ArchitectureTests/HJ.Server.ArchitectureTests.csproj"

Write-Host "Fixing ArchitectureTests for Central Package Management..."

dotnet remove $project package coverlet.collector
dotnet remove $project package Microsoft.NET.Test.Sdk
dotnet remove $project package xunit
dotnet remove $project package xunit.runner.visualstudio

dotnet add $project package xunit
dotnet add $project package Microsoft.NET.Test.Sdk
dotnet add $project package xunit.runner.visualstudio
dotnet add $project package coverlet.collector
dotnet add $project package NetArchTest.Rules

Write-Host "Fix completed."