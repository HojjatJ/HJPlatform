using HJ.Server.Domain.Common;
namespace HJ.Server.Domain.Installations;


public class Installation : BaseEntity
{
    

    public Guid InstallationId { get; set; }

    public string AppId { get; set; } = default!;

    public string CurrentVersion { get; set; } = default!;

    public DateTime FirstSeenAt { get; set; }

    public DateTime LastSeenAt { get; set; }
}

