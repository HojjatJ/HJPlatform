using System;
using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Domain.Installations;

public interface IInstallationRepository
{
    Task<Installation?> GetByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken = default);
    Task AddAsync(Installation installation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Installation installation, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid installationId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}