using System;

namespace HJ.Server.Contracts.Installations;

public class InstallationDto
{
    public Guid Id { get; set; }
    public Guid InstallationId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductVersionId { get; set; }
    public DateTime FirstSeenAt { get; set; }
    public DateTime LastSeenAt { get; set; }
    public InstallationEnvironmentDto? Environment { get; set; }
}