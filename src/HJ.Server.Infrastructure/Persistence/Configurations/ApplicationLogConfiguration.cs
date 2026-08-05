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
