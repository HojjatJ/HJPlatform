using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HJ.Server.Foundation.Abstractions.Telemetry;
using HJ.Server.Domain.Telemetry;
using HJ.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HJ.Server.Infrastructure.Telemetry;

public class TelemetryService : ITelemetryService
{
    private readonly HJDbContext _dbContext;
    private readonly ILogger<TelemetryService> _logger;

    public TelemetryService(HJDbContext dbContext, ILogger<TelemetryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task TrackEventAsync(
        string eventName,
        int version,
        object payload,
        Guid? installationId,
        Guid? operationId,
        CancellationToken cancellationToken)
    {
        try
        {
            var telemetryEvent = TelemetryEvent.Create(
                eventName,
                version,
                JsonSerializer.Serialize(payload),
                installationId ?? Guid.Empty,
                operationId ?? Guid.Empty
            );

            _dbContext.TelemetryEvents.Add(telemetryEvent);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist telemetry event {EventName}", eventName);
        }
    }
}
