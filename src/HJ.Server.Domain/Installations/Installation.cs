using System;
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Installations;

public class Installation : BaseEntity
{
    public Guid InstallationId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid ProductVersionId { get; private set; }
    public DateTime FirstSeenAt { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    public InstallationEnvironment? Environment { get; private set; }

    private Installation()
    {
    }

    private Installation(
        Guid installationId,
        Guid productId,
        Guid productVersionId,
        Guid? tenantId)
    {
        Id = Guid.NewGuid();
        InstallationId = installationId;
        ProductId = productId;
        ProductVersionId = productVersionId;
        TenantId = tenantId;

        var now = DateTime.UtcNow;
        FirstSeenAt = now;
        LastSeenAt = now;
        CreatedAt = now;
    }

    public static Installation Create(
        Guid installationId,
        Guid productId,
        Guid productVersionId,
        Guid? tenantId)
    {
        if (installationId == Guid.Empty)
            throw new ArgumentException("InstallationId cannot be empty.", nameof(installationId));

        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(productId));

        if (productVersionId == Guid.Empty)
            throw new ArgumentException("ProductVersionId cannot be empty.", nameof(productVersionId));

        return new Installation(
            installationId,
            productId,
            productVersionId,
            tenantId);
    }

    public void RecordHeartbeat()
    {
        LastSeenAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateVersion(Guid newProductVersionId)
    {
        if (newProductVersionId == Guid.Empty)
            throw new ArgumentException("ProductVersionId cannot be empty.", nameof(newProductVersionId));

        ProductVersionId = newProductVersionId;
        RecordHeartbeat();
    }

    public void SetEnvironment(InstallationEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);

        Environment = environment;
        ModifiedAt = DateTime.UtcNow;
    }
}