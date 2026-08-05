$root = Split-Path -Parent $PSScriptRoot

$files = Get-ChildItem `
    "$root\src\HJ.Server.Infrastructure" `
    -Filter "*.cs" `
    -Recurse


$target = $files |
    Where-Object {
        $_.Name -like "*Dependency*" -or
        $_.Name -like "*ServiceCollection*"
    } |
    Select-Object -First 1


if (-not $target)
{
    Write-Host "Infrastructure DI file not found."
    exit
}


$content = Get-Content $target.FullName -Raw


if ($content -notmatch "IProductRepository")
{
    $content += @"

using HJ.Server.Domain.Products;
using HJ.Server.Infrastructure.Persistence.Repositories;

namespace HJ.Server.Infrastructure;

public static partial class DependencyInjection
{
    private static void AddProductRepositories(IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
"@

    Set-Content $target.FullName $content
}


Write-Host "Product repository DI registered."