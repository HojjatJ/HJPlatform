namespace HJ.Server.Domain.Logging;


public class ApplicationLog
{
    public Guid Id { get; set; }

    public Guid InstallationId { get; set; }

    public Guid OperationId { get; set; }

    public string Level { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string? ExceptionJson { get; set; }

    public string? PropertiesJson { get; set; }

    public DateTime CreatedAt { get; set; }
}
