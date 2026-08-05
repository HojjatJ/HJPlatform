$root = "D:\Projects\Visual Studio\HJPlatform"

$program = Join-Path $root "src\HJ.Server.Api\Program.cs"

$content = Get-Content $program -Raw

if ($content -notmatch "AddHJApplication") {

$content = $content -replace `
"var builder = WebApplication.CreateBuilder\(args\);", `
@"
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHJApplication();
"@

}

Set-Content $program $content -Encoding UTF8


$appFolder = Join-Path $root "src\HJ.Server.Application"

$diFile = Join-Path $appFolder "DependencyInjection\ServiceCollectionExtensions.cs"

New-Item (Split-Path $diFile) -ItemType Directory -Force | Out-Null

@"
using Microsoft.Extensions.DependencyInjection;

namespace HJ.Server.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHJApplication(
        this IServiceCollection services)
    {
        services.AddScoped<Products.IProductService, Products.ProductService>();

        return services;
    }
}
"@ | Set-Content $diFile -Encoding UTF8

Write-Host "Application DI registered."