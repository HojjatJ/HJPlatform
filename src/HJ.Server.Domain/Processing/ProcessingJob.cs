namespace HJ.Server.Domain.Processing;


public class ProcessingJob
{
    public Guid Id { get; set; }

    public Guid OperationId { get; set; }

    public string BatchId { get; set; } = default!;

    public string ExecutionSource { get; set; } = default!;

    public int FilesCount { get; set; }

    public int SuccessCount { get; set; }

    public int FailedCount { get; set; }

    public long TargetSizeKB { get; set; }

    public long SavedBytes { get; set; }

    public long DurationMs { get; set; }

    public string? ProcessingMode { get; set; }

    public int ConcurrencyLevel { get; set; }

    public DateTime CreatedAt { get; set; }
}
