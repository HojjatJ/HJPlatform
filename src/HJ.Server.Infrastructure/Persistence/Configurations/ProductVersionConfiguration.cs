using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HJ.Server.Domain.Products;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class ProductVersionConfiguration : IEntityTypeConfiguration<ProductVersion>
{
    public void Configure(EntityTypeBuilder<ProductVersion> builder)
    {
        builder.ToTable("ProductVersions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Version)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ReleaseNotes)
            .HasMaxLength(5000);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ProductId, x.Version })
            .IsUnique();
    }
}
