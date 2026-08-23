using HJ.Server.Domain.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Infrastructure.Persistence;

namespace HJ.Server.Infrastructure.Persistence.Repositories;

public class OperationRepository : IOperationRepository
{
    private readonly HJDbContext _context;

    public OperationRepository(HJDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Operation operation, CancellationToken cancellationToken)
    {
        await _context.Operations.AddAsync(operation, cancellationToken);
    }

    public async Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Operations.FindAsync(new object[] { id }, cancellationToken);
    }

    public Task UpdateAsync(Operation operation, CancellationToken cancellationToken)
    {
        _context.Operations.Update(operation);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
