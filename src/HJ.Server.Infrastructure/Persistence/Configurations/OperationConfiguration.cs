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
