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
