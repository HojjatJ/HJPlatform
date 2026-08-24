using System;
using System.Collections.Generic;
using HJ.Server.Domain.Common;

namespace HJ.Server.Domain.Operations;

public class Operation : BaseEntity
{
    public Guid InstallationId { get; set; }
    public Guid CorrelationId { get; set; }
    public string Type { get; set; } = default!;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public OperationStatus Status { get; set; }

    public ICollection<OperationExecution> Executions { get; set; } = new List<OperationExecution>();

    public static Operation Create(Guid installationId, string type, Guid? tenantId)
    {
        return new Operation
        {
            Id = Guid.NewGuid(),
            InstallationId = installationId,
            Type = type,
            StartedAt = DateTime.UtcNow,
            Status = OperationStatus.Started,
            CreatedAt = DateTime.UtcNow,
            TenantId = tenantId
        };
    }

    public void Complete(OperationStatus status)
    {
        Status = status;
        EndedAt = DateTime.UtcNow;
    }
}
