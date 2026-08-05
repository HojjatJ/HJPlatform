$root = Split-Path -Parent $PSScriptRoot

$configPath = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\Configurations"

New-Item -ItemType Directory -Force -Path $configPath | Out-Null

@'
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HJ.Server.Domain.Tenancy;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
'@ | Set-Content "$configPath\TenantConfiguration.cs"


@'
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HJ.Server.Domain.Products;

namespace HJ.Server.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.TenantId, x.Code })
            .IsUnique();
    }
}
'@ | Set-Content "$configPath\ProductConfiguration.cs"


@'
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
'@ | Set-Content "$configPath\ProductVersionConfiguration.cs"


Write-Host "Product EF configurations created."