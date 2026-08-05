using HJ.Server.Domain.Products;
using HJ.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HJ.Server.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HJDbContext _dbContext;

    public ProductRepository(HJDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Product?> GetByCodeAsync(
        string code,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .FirstOrDefaultAsync(
                x => x.Code == code &&
                     x.TenantId == tenantId,
                cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(
            product,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        string code,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AnyAsync(
                x => x.Code == code &&
                     x.TenantId == tenantId,
                cancellationToken);
    }
}
