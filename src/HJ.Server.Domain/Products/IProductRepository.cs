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
