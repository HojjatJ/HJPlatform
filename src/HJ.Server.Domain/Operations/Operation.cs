namespace HJ.Server.Domain.Operations;


public class Operation
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public Guid CorrelationId { get; set; }

    public string Type { get; set; } = default!;

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string Status { get; set; } = default!;
}
