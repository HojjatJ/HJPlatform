$root = "D:\Projects\Visual Studio\HJPlatform"

$content = @"
<Project>

  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>

    <!-- API -->
    <PackageVersion Include="FastEndpoints" Version="8.2.0" />
    <PackageVersion Include="FastEndpoints.Swagger" Version="8.2.0" />
    <PackageVersion Include="Scalar.AspNetCore" Version="2.16.17" />
    <PackageVersion Include="Serilog.AspNetCore" Version="10.0.0" />

    <!-- EF Core -->
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="10.0.10" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.10" />
    <PackageVersion Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.3" />

    <!-- Application -->
    <PackageVersion Include="FluentValidation" Version="12.1.1" />
    <PackageVersion Include="Riok.Mapperly" Version="4.3.1" />

    <!-- Foundation -->
    <PackageVersion Include="Ardalis.Result" Version="10.1.0" />

    <!-- Testing -->
    <PackageVersion Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.10" />
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="18.8.1" />
    <PackageVersion Include="xunit" Version="2.9.3" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="3.1.5" />
    <PackageVersion Include="FluentAssertions" Version="8.10.0" />
    <PackageVersion Include="coverlet.collector" Version="6.0.4" />
    <PackageVersion Include="Moq" Version="4.20.72" />
    <PackageVersion Include="Testcontainers.PostgreSql" Version="4.13.0" />

  </ItemGroup>

</Project>
"@

Set-Content "$root\Directory.Packages.props" $content

Write-Host "Central package versions updated."