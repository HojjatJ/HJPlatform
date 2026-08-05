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
