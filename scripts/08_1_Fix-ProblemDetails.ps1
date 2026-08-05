$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


Write-Host "Removing Hellang package..."

dotnet remove ".\src\HJ.Server.Api\HJ.Server.Api.csproj" package Hellang.Middleware.ProblemDetails


Write-Host "Updating Program.cs..."

$file = ".\src\HJ.Server.Api\Program.cs"

$content = Get-Content $file -Raw


$content = $content.Replace(
"using Hellang.Middleware.ProblemDetails;
",
"")


$content = $content.Replace(
"app.UseProblemDetails();
",
"")


Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8


Write-Host ""
Write-Host "ProblemDetails fixed using built-in ASP.NET Core implementation."