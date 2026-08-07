using System;
using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByCodeAsync(string code, Guid? tenantId = null, CancellationToken cancellationToken = default);
    Task AddProductAsync(Product product, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
