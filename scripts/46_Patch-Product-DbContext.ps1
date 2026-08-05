$root = Split-Path -Parent $PSScriptRoot

$dbContextPath = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\HJDbContext.cs"

if (!(Test-Path $dbContextPath)) {
    throw "HJDbContext.cs not found: $dbContextPath"
}

$content = Get-Content $dbContextPath -Raw

if ($content -notmatch "using HJ.Server.Domain.Products;")
{
    $content = "using HJ.Server.Domain.Products;`r`nusing HJ.Server.Domain.Tenancy;`r`n" + $content
}

if ($content -notmatch "DbSet<Product>")
{
    $marker = "{"

    $insert = @"

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductVersion> ProductVersions => Set<ProductVersion>();

    public DbSet<Tenant> Tenants => Set<Tenant>();

"@

    $index = $content.IndexOf($marker)

    if ($index -ge 0)
    {
        $content = $content.Insert($index + 1, $insert)
    }
}

Set-Content $dbContextPath $content

Write-Host "HJDbContext patched for Product domain."