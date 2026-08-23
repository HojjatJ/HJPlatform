using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Operations;

public class OperationExecution : BaseEntity
{
    public Guid OperationId { get; private set; }
    public string ExecutionSource { get; private set; } = default!;
    public string? ExecutionMode { get; private set; }
    public int ItemsCount { get; private set; }
    public int SucceededCount { get; private set; }
    public int FailedCount { get; private set; }
    public int ConcurrencyLevel { get; private set; }
    public long DurationMs { get; private set; }
    public string? MetadataJson { get; private set; }

    private OperationExecution() { }

    public static OperationExecution Create(
        Guid operationId, 
        string executionSource, 
        int itemsCount, 
        int succeededCount, 
        int failedCount, 
        long durationMs)
    {
        return new OperationExecution
        {
            Id = Guid.NewGuid(),
            OperationId = operationId,
            ExecutionSource = executionSource,
            ItemsCount = itemsCount,
            SucceededCount = succeededCount,
            FailedCount = failedCount,
            DurationMs = durationMs,
            CreatedAt = DateTime.UtcNow
        };
    }
}
