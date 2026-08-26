using HJ.Server.Domain.Installations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class InstallationConfiguration : IEntityTypeConfiguration<Installation>
{
    public void Configure(EntityTypeBuilder<Installation> builder)
    {
        builder.ToTable("Installations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InstallationId)
            .IsRequired();

        builder.HasIndex(x => x.InstallationId)
            .IsUnique();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.ProductVersionId)
            .IsRequired();

        builder.Property(x => x.FirstSeenAt)
            .IsRequired();

        builder.Property(x => x.LastSeenAt)
            .IsRequired();

        builder.OwnsOne(x => x.Environment, env =>
        {
            env.ToTable("InstallationEnvironments");

            env.Property(x => x.OSVersion).HasMaxLength(200);
            env.Property(x => x.CpuName).HasMaxLength(200);
            env.Property(x => x.ScreenResolution).HasMaxLength(50);
            env.Property(x => x.HardwareIdentifier).HasMaxLength(250);

            env.WithOwner()
               .HasForeignKey("InstallationId");

            env.HasKey("InstallationId");

            env.Ignore(x => x.Id);
        });
    }
}