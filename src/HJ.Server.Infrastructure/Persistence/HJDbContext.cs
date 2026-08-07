using Microsoft.EntityFrameworkCore;
using HJ.Server.Domain.Products;
using HJ.Server.Domain.Installations;

namespace HJ.Server.Infrastructure.Persistence;

public class HJDbContext : DbContext
{
    public HJDbContext(DbContextOptions<HJDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Installation> Installations => Set<Installation>();
    public DbSet<InstallationEnvironment> InstallationEnvironments => Set<InstallationEnvironment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HJDbContext).Assembly);
    }
}