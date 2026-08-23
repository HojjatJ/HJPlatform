using HJ.Server.Domain.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class OperationExecutionConfiguration 
    : IEntityTypeConfiguration<OperationExecution>
{
    public void Configure(EntityTypeBuilder<OperationExecution> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne<Operation>()
            .WithMany(o => o.Executions)
            .HasForeignKey(x => x.OperationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.ExecutionSource)
            .HasMaxLength(100);
    }
}
