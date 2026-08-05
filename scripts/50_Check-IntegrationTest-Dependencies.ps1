$root = Split-Path -Parent $PSScriptRoot

dotnet list "$root\tests\HJ.Server.IntegrationTests\HJ.Server.IntegrationTests.csproj" package --include-transitive |
Select-String "EntityFrameworkCore|Relational|Npgsql|Microsoft.AspNetCore"