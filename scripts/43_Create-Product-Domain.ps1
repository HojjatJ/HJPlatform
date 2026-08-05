$root = Split-Path -Parent $PSScriptRoot

$domain = Join-Path $root "src\HJ.Server.Domain"

$folders = @(
    "$domain\Common",
    "$domain\Tenancy",
    "$domain\Products\Enums"
)

foreach ($folder in $folders) {
    New-Item -ItemType Directory -Force -Path $folder | Out-Null
}

@'
namespace HJ.Server.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public Guid? TenantId { get; protected set; }

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? ModifiedAt { get; protected set; }
}
'@ | Set-Content "$domain\Common\BaseEntity.cs"


@'
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Tenancy;

public class Tenant : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    private Tenant()
    {
    }

    public Tenant(string name, string code)
    {
        Name = name;
        Code = code;
    }
}
'@ | Set-Content "$domain\Tenancy\Tenant.cs"


@'
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Products;

public class Product : BaseEntity
{
    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; } = true;

    private Product()
    {
    }

    public Product(string code, string name, string? description = null)
    {
        Code = code;
        Name = name;
        Description = description;
    }
}
'@ | Set-Content "$domain\Products\Product.cs"


@'
using HJ.Server.Domain.Common;
using HJ.Server.Domain.Products.Enums;

namespace HJ.Server.Domain.Products;

public class ProductVersion : BaseEntity
{
    public Guid ProductId { get; private set; }

    public string Version { get; private set; } = null!;

    public string? BuildNumber { get; private set; }

    public string? ReleaseNotes { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public VersionStatus Status { get; private set; }

    public UpdatePolicy UpdatePolicy { get; private set; }

    private ProductVersion()
    {
    }

    public ProductVersion(Guid productId, string version)
    {
        ProductId = productId;
        Version = version;
        ReleaseDate = DateTime.UtcNow;
        Status = VersionStatus.Draft;
        UpdatePolicy = UpdatePolicy.Optional;
    }
}
'@ | Set-Content "$domain\Products\ProductVersion.cs"


@'
namespace HJ.Server.Domain.Products.Enums;

public enum VersionStatus
{
    Draft = 0,
    Published = 1,
    Deprecated = 2,
    Retired = 3
}
'@ | Set-Content "$domain\Products\Enums\VersionStatus.cs"


@'
namespace HJ.Server.Domain.Products.Enums;

public enum UpdatePolicy
{
    Optional = 0,
    Recommended = 1,
    Required = 2,
    Blocked = 3
}
'@ | Set-Content "$domain\Products\Enums\UpdatePolicy.cs"


Write-Host "Product domain entities created."