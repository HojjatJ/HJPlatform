using System;
using System.Threading;
using System.Threading.Tasks;

namespace HJ.Server.Foundation.Abstractions.Telemetry;

public interface ITelemetryService
{
    Task TrackEventAsync(
        string eventName,
        int version,
        object payload,
        Guid? installationId,
        Guid? operationId,
        CancellationToken cancellationToken);
}
