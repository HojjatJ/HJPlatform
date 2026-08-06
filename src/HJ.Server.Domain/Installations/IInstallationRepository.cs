using System;
using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Domain.Installations;

public interface IInstallationRepository
{
    Task<Installation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Installation?> GetByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken = default);
    Task AddAsync(Installation installation, CancellationToken cancellationToken = default);
}