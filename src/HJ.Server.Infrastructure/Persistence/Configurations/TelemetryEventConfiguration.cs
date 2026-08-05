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
