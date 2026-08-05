$root = "D:\Projects\Visual Studio\HJPlatform"

$file = Join-Path $root "Directory.Build.props"

$content = @"
<Project>

  <PropertyGroup>

    <TargetFramework>net10.0</TargetFramework>

    <ImplicitUsings>enable</ImplicitUsings>

    <Nullable>enable</Nullable>

    <LangVersion>latest</LangVersion>

    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>

    <Deterministic>true</Deterministic>

    <Company>HJPlatform</Company>

    <Authors>HJPlatform</Authors>

  </PropertyGroup>


  <PropertyGroup Condition="'`$(Configuration)' == 'Release'">

    <DebugType>embedded</DebugType>

    <DebugSymbols>true</DebugSymbols>

  </PropertyGroup>


</Project>
"@

Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8


Write-Host "Created Directory.Build.props"