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

        builder.HasOne(x => x.Environment)
            .WithOne()
            .HasForeignKey<InstallationEnvironment>(x => x.InstallationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}