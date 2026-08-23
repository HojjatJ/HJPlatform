using Microsoft.EntityFrameworkCore;
using HJ.Server.Domain.Products;
using HJ.Server.Domain.Installations;
using HJ.Server.Domain.Operations;
using HJ.Server.Domain.Logging;
using HJ.Server.Domain.Telemetry;
using HJ.Server.Domain.Tenancy;

namespace HJ.Server.Infrastructure.Persistence;

public class HJDbContext : DbContext
{
    public HJDbContext(DbContextOptions<HJDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Installation> Installations => Set<Installation>();
    public DbSet<InstallationEnvironment> InstallationEnvironments => Set<InstallationEnvironment>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<OperationExecution> OperationExecutions => Set<OperationExecution>();
    public DbSet<ApplicationLog> ApplicationLogs => Set<ApplicationLog>();
    public DbSet<TelemetryEvent> TelemetryEvents => Set<TelemetryEvent>();
    public DbSet<Tenant> Tenants => Set<Tenant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HJDbContext).Assembly);
    }
}
