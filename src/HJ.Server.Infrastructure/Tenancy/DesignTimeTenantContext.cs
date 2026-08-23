using HJ.Server.Foundation.Tenancy;

namespace HJ.Server.Infrastructure.Tenancy;

internal sealed class DesignTimeTenantContext : ITenantContext
{
    public Guid? TenantId => null;
}
