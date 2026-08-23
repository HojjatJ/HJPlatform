using System;

namespace HJ.Server.Foundation.Tenancy;

public interface ITenantContext
{
    Guid? TenantId { get; }
}
