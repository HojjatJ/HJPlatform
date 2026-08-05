$root = "D:\Projects\Visual Studio\HJPlatform"

$content = @"
<Project>

  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>

    <!-- Entity Framework -->
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="10.0.10" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Relational" Version="10.0.10" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.10" />

    <!-- Testing -->
    <PackageVersion Include="xunit" Version="2.9.3" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="3.1.5" />
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="18.8.1" />
    <PackageVersion Include="FluentAssertions" Version="8.10.0" />

  </ItemGroup>

</Project>
"@

Set-Content "$root\Directory.Packages.props" $content

Write-Host "Central package version file created."
