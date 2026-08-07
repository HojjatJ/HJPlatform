using System;

namespace HJ.Server.Contracts.Installations;

public class RegisterInstallationRequest
{
    public Guid InstallationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductVersionId { get; set; }
    public Guid? TenantId { get; set; }
}