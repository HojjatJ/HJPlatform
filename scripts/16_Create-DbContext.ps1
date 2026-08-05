$root = "D:\Projects\Visual Studio\HJPlatform"

$persistencePath = "$root\src\HJ.Server.Infrastructure\Persistence"


$folders = @(
    "$persistencePath",
    "$persistencePath\Configurations"
)


foreach($folder in $folders)
{
    if(!(Test-Path $folder))
    {
        New-Item -ItemType Directory -Path $folder | Out-Null
    }
}



@"
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
"@ | Set-Content `
"$persistencePath\HJDbContext.cs" `
-Encoding UTF8



@"
using HJ.Server.Domain.Installations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class InstallationConfiguration 
    : IEntityTypeConfiguration<Installation>
{
    public void Configure(EntityTypeBuilder<Installation> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasIndex(x => x.InstallationId)
            .IsUnique();


        builder.HasIndex(x => x.AppId);


        builder.Property(x => x.AppId)
            .HasMaxLength(100);


        builder.Property(x => x.CurrentVersion)
            .HasMaxLength(50);
    }
}
"@ | Set-Content `
"$persistencePath\Configurations\InstallationConfiguration.cs" `
-Encoding UTF8



@"
using HJ.Server.Domain.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class OperationConfiguration 
    : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasIndex(x => x.CorrelationId);

        builder.HasIndex(x => x.InstallationId);

        builder.HasIndex(x => x.Type);

        builder.HasIndex(x => x.StartedAt);


        builder.Property(x => x.Type)
            .HasMaxLength(100);


        builder.Property(x => x.Status)
            .HasMaxLength(50);
    }
}
"@ | Set-Content `
"$persistencePath\Configurations\OperationConfiguration.cs" `
-Encoding UTF8



@"
using HJ.Server.Domain.Telemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class TelemetryEventConfiguration 
    : IEntityTypeConfiguration<TelemetryEvent>
{
    public void Configure(EntityTypeBuilder<TelemetryEvent> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasIndex(x => x.OperationId);

        builder.HasIndex(x => x.InstallationId);

        builder.HasIndex(x => new
        {
            x.EventName,
            x.CreatedAt
        });


        builder.Property(x => x.EventName)
            .HasMaxLength(150);
    }
}
"@ | Set-Content `
"$persistencePath\Configurations\TelemetryEventConfiguration.cs" `
-Encoding UTF8



@"
using HJ.Server.Domain.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class ApplicationLogConfiguration 
    : IEntityTypeConfiguration<ApplicationLog>
{
    public void Configure(EntityTypeBuilder<ApplicationLog> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasIndex(x => x.OperationId);

        builder.HasIndex(x => x.InstallationId);

        builder.HasIndex(x => x.Level);

        builder.HasIndex(x => x.CreatedAt);


        builder.Property(x => x.Level)
            .HasMaxLength(30);
    }
}
"@ | Set-Content `
"$persistencePath\Configurations\ApplicationLogConfiguration.cs" `
-Encoding UTF8



@"
using HJ.Server.Domain.Optimization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class OptimizationBatchConfiguration 
    : IEntityTypeConfiguration<OptimizationBatch>
{
    public void Configure(EntityTypeBuilder<OptimizationBatch> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasIndex(x => x.OperationId);

        builder.HasIndex(x => x.BatchId)
            .IsUnique();


        builder.Property(x => x.BatchId)
            .HasMaxLength(20);


        builder.Property(x => x.ExecutionSource)
            .HasMaxLength(50);


        builder.Property(x => x.ProcessingMode)
            .HasMaxLength(50);
    }
}
"@ | Set-Content `
"$persistencePath\Configurations\OptimizationBatchConfiguration.cs" `
-Encoding UTF8



Write-Host "DbContext and configurations created."