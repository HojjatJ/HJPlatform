$root = "D:\Projects\Visual Studio\HJPlatform"

$file = Join-Path $root "src\HJ.Server.Api\Program.cs"

$content = Get-Content $file -Raw

if ($content -notmatch "HJ.Server.Application.DependencyInjection")
{
    $content = "using HJ.Server.Application.DependencyInjection;`r`n" + $content
}

Set-Content $file $content -Encoding UTF8

Write-Host "Api using added."