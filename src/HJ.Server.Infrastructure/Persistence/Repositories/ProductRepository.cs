using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HJ.Server.Domain.Products;
using HJ.Server.Infrastructure.Persistence;

namespace HJ.Server.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HJDbContext _context;

    public ProductRepository(HJDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByCodeAsync(string code, Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Product>().AsQueryable();

        if (tenantId.HasValue)
        {
            // query = query.Where(p => p.TenantId == tenantId.Value);
        }

        return await query.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Set<Product>().AddAsync(product, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
