using HJ.Server.Domain.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;


public class ProcessingJobConfiguration 
    : IEntityTypeConfiguration<ProcessingJob>
{
    public void Configure(EntityTypeBuilder<ProcessingJob> builder)
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

