using HJ.Server.Domain.Installations;
using HJ.Server.Domain.Logging;
using HJ.Server.Domain.Operations;
using HJ.Server.Domain.Optimization;
using HJ.Server.Domain.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace HJ.Server.Infrastructure.Persistence;


public class HJDbContext : DbContext
{
    public HJDbContext(DbContextOptions<HJDbContext> options)
        : base(options)
    {
    }


    public DbSet<Installation> Installations => Set<Installation>();

    public DbSet<InstallationEnvironment> InstallationEnvironments => Set<InstallationEnvironment>();

    public DbSet<Operation> Operations => Set<Operation>();

    public DbSet<TelemetryEvent> TelemetryEvents => Set<TelemetryEvent>();

    public DbSet<ApplicationLog> ApplicationLogs => Set<ApplicationLog>();

    public DbSet<OptimizationBatch> OptimizationBatches => Set<OptimizationBatch>();



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.ApplyConfigurationsFromAssembly(
            typeof(HJDbContext).Assembly);
    }
}
