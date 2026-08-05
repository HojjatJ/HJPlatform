$root = "D:\Projects\Visual Studio\HJPlatform"

$file = Join-Path $root "src\HJ.Server.Infrastructure\DependencyInjection\ServiceCollectionExtensions.cs"

$content = Get-Content $file -Raw

$content = $content -replace `
"return services;", `
@"
services.AddScoped<HJ.Server.Domain.Products.IProductRepository,
    HJ.Server.Infrastructure.Persistence.Repositories.ProductRepository>();

return services;
"@

Set-Content $file $content -Encoding UTF8

Write-Host "Repository DI registered."