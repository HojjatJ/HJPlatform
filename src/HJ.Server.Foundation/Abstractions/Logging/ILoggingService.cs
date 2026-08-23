using System;
using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Foundation.Abstractions.Logging;

public interface ILoggingService
{
    Task LogAsync(
        string level,
        string message,
        Guid? installationId,
        Guid? operationId,
        object? exception,
        object? properties,
        CancellationToken cancellationToken);
}
