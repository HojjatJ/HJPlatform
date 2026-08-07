using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HJ.Server.Domain.Installations;

namespace HJ.Server.Infrastructure.Persistence.Repositories;

public class InstallationRepository : IInstallationRepository
{
    private readonly HJDbContext _dbContext;

    public InstallationRepository(HJDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Installation?> GetByInstallationIdAsync(Guid installationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Installations
            .Include(i => i.Environment)
            .FirstOrDefaultAsync(i => i.InstallationId == installationId, cancellationToken);
    }

    public async Task AddAsync(Installation installation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Installations.AddAsync(installation, cancellationToken);
    }

    public async Task UpdateAsync(Installation installation, CancellationToken cancellationToken = default)
    {
        _dbContext.Installations.Update(installation);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid installationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Installations
            .AnyAsync(i => i.InstallationId == installationId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}