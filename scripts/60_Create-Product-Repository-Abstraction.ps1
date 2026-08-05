$root = Split-Path -Parent $PSScriptRoot

$path = "$root\src\HJ.Server.Domain\Products"

New-Item -ItemType Directory -Path $path -Force | Out-Null

@"
using HJ.Server.Domain.Products;

namespace HJ.Server.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Product?> GetByCodeAsync(
        string code,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string code,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);
}
"@ | Set-Content "$path\IProductRepository.cs"

Write-Host "Product repository abstraction created."