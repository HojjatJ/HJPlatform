using HJ.Server.Domain.Installations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class InstallationEnvironmentConfiguration : IEntityTypeConfiguration<InstallationEnvironment>
{
    public void Configure(EntityTypeBuilder<InstallationEnvironment> builder)
    {
        builder.ToTable("InstallationEnvironments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InstallationId)
            .IsRequired();

        builder.Property(x => x.OSVersion)
            .HasMaxLength(200);

        builder.Property(x => x.CpuName)
            .HasMaxLength(200);

        builder.Property(x => x.ScreenResolution)
            .HasMaxLength(50);

        builder.Property(x => x.HardwareIdentifier)
            .HasMaxLength(250);
    }
}