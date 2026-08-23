using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Domain.Operations;

public interface IOperationRepository
{
    Task AddAsync(Operation operation, CancellationToken cancellationToken);
    Task<Operation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Operation operation, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
